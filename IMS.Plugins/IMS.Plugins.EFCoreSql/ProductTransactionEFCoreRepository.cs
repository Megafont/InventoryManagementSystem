using System;
using System.Collections.Generic;
using System.Text;
using IMS.CoreBusiness;
using IMS.CoreBusiness.Validations;
using IMS.Plugins.EFCoreSqlServer;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugins.InMemory
{
	public class ProductTransactionEFCoreRepository : IProductTransactionRepository
	{
		private readonly IDbContextFactory<IMS_Db_Context> _contextFactory;
		private readonly IProductRepository _productRepository;
		private readonly IInventoryTransactionRepository _inventoryTransactionRepository;
		private readonly IInventoryRepository _inventoryRepository;

		public ProductTransactionEFCoreRepository(
			IDbContextFactory<IMS_Db_Context> contextFactory,
			IProductRepository productRepository,
			IInventoryTransactionRepository inventoryTransactionRepository,
			IInventoryRepository inventoryRepository)
		{
			_contextFactory = contextFactory;
			_productRepository = productRepository;
			_inventoryTransactionRepository = inventoryTransactionRepository;
			_inventoryRepository = inventoryRepository;
		}

		public async Task ProduceAsync(string productionNumber, Product product, int quantity, string producedBy)
		{
			// Decrease the Inventory Quantities
			// ---------------------------------------------------------------------------------------------------------------------------

			// IMPORANT:
			//	   If you see that a produce product operation did not generate the inventory transactions for each affected inventory,
			//     check that the product has its inventories specified. If not, then no inventories will be affected since
			//     the product object has no inventory requirements set.
			//
			//     TODO: The best solution would probably be to make it so the Product class requires that at least one inventory is specified since it doesn't make sense to have a product that can be produced from nothing.

			using IMS_Db_Context db = _contextFactory.CreateDbContext();

			// We need to get the full Product object from the repo. this is because the one passed in came from the edit form
			// on the ProduceProduct page, so it is not complete.
			Product fullProduct = await _productRepository.GetProductByIdAsync(product.ProductID);
			if (fullProduct != null)
			{
				foreach (ProductInventory productInventory in fullProduct.ProductInventories)
				{
					if (productInventory.Inventory != null)
					{
						// Add inventory transaction for this inventory.
						_inventoryTransactionRepository.ProduceAsync(
							productionNumber,
							productInventory.Inventory,
							productInventory.InventoryQuantity * quantity,
							producedBy,
							null);

						// Decrease the quantity of this inventory.
						Inventory inventory = await _inventoryRepository.GetInventoryByIdAsync(productInventory.InventoryID);
						inventory.Quantity -= productInventory.InventoryQuantity * quantity;
						await _inventoryRepository.UpdateInventoryAsync(inventory);
					}
				}
			}

			// Add Production Transaction
			// ---------------------------------------------------------------------------------------------------------------------------
			db.ProductTransactions?.Add(new ProductTransaction
			{
				ProductionNumber = productionNumber,
				ProductID = product.ProductID,
				QuantityBefore = fullProduct.Quantity,
				ActivityType = ProductTransactionTypes.ProduceProduct,
				QuantityAfter = fullProduct.Quantity + quantity,
				TransactionDate = DateTime.Now,
				DoneBy = producedBy,
				UnitPrice = product.Price,
			});

			await db.SaveChangesAsync();
		}

		public async Task SellProductAsync(string salesOrderNumber, Product product, int quantity, decimal unitPrice,
			string soldBy)
		{
			using IMS_Db_Context db = _contextFactory.CreateDbContext();

			db.ProductTransactions?.Add(new ProductTransaction
			{
				ProductionNumber = salesOrderNumber,
				ProductID = product.ProductID,
				QuantityBefore = product.Quantity,
				ActivityType = ProductTransactionTypes.SellProduct,
				QuantityAfter = product.Quantity - quantity,
				TransactionDate = DateTime.Now,
				DoneBy = soldBy,
				UnitPrice = unitPrice,
			});

			await db.SaveChangesAsync();
		}

		public async Task<IEnumerable<ProductTransaction>> GetProductTransactionsAsync(string ProductName, DateTime? dateFrom, DateTime? dateTo,
			ProductTransactionTypes? transactionType)
		{
			using IMS_Db_Context db = _contextFactory.CreateDbContext();

			// Use linq to filter the Product transactions based on the passed in filtering options.
			var results = from productTransaction in db.ProductTransactions
				join Product in db.Products on productTransaction.ProductID equals Product.ProductID
				where
					(string.IsNullOrWhiteSpace(ProductName) ||
					 Product.ProductName.ToLower().IndexOf(ProductName.ToLower()) >= 0) &&
					(!dateFrom.HasValue || productTransaction.TransactionDate >= dateFrom.Value.Date) &&
					(!dateTo.HasValue || productTransaction.TransactionDate <= dateTo.Value.Date) &&
					(!transactionType.HasValue || productTransaction.ActivityType == transactionType)
				select productTransaction;

			// Return the search results.
			// The Include() call is telling Entity Framework Core that we want the returned product transactions
			// to include the Product records they link to.
			return await results.Include(x => x.Product).ToListAsync();
		}
	}
}
