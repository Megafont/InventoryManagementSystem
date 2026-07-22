using System;
using System.Collections.Generic;
using System.Text;
using IMS.CoreBusiness;
using IMS.CoreBusiness.Validations;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory
{
	public class ProductTransactionRepository : IProductTransactionRepository
	{
		private List<ProductTransaction> _productTransactions = new();

		private readonly IProductRepository _productRepository;
		private readonly IInventoryTransactionRepository _inventoryTransactionRepository;
		private readonly IInventoryRepository _inventoryRepository;

		public ProductTransactionRepository(IProductRepository productRepository,
			IInventoryTransactionRepository inventoryTransactionRepository,
			IInventoryRepository inventoryRepository)
		{
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
			_productTransactions.Add(new ProductTransaction
			{
				ProductionNumber = productionNumber,
				ProductID = product.ProductID,
				QuantityBefore = fullProduct.Quantity,
				ActivityType = ProductTransactionTypes.ProduceProduct,
				QuantityAfter = fullProduct.Quantity + quantity,
				TransactionDate = DateTime.Now,
				DoneBy = producedBy,
			});

		}

		public Task SellProductAsync(string salesOrderNumber, Product product, int quantity, decimal unitPrice, string soldBy)
		{
			_productTransactions.Add(new ProductTransaction
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

			// We return this here since this function is actually synchronous, but it uses the name SellProductAsync() because
			// that is enforced by the interface. This is why the ProduceProductAsync() method above does not end with the line
			// below (return Task.CompletedTask;). That is handled for you in a true asynchronous method.
			return Task.CompletedTask;
		}

		public async Task<IEnumerable<ProductTransaction>> GetProductTransactionsAsync(string ProductName, DateTime? dateFrom, DateTime? dateTo,
			ProductTransactionTypes? transactionType)
		{
			// You wouldn't normally grab all the transactions like this, but this is an in-memory repository, so it's ok in this case.
			List<Product> products = (await _productRepository.GetProductsByNameAsync(string.Empty)).ToList();

			// Use linq to filter the Product transactions based on the passed in filtering options.
			var results = from productTransaction in _productTransactions
				join Product in products on productTransaction.ProductID equals Product.ProductID
				where
					(string.IsNullOrWhiteSpace(ProductName) || Product.ProductName.ToLower().IndexOf(ProductName.ToLower()) >= 0) &&
					(!dateFrom.HasValue || productTransaction.TransactionDate >= dateFrom.Value.Date) &&
					(!dateTo.HasValue || productTransaction.TransactionDate <= dateTo.Value.Date) &&
					(!transactionType.HasValue || productTransaction.ActivityType == transactionType)
				select new ProductTransaction
				{
					Product = Product,
					ProductionTransactionID = productTransaction.ProductionTransactionID,
					SalesOrderNumber = productTransaction.SalesOrderNumber,
					ProductionNumber = productTransaction.ProductionNumber,
					ProductID = productTransaction.ProductID,
					QuantityBefore = productTransaction.QuantityBefore,
					ActivityType = productTransaction.ActivityType,
					QuantityAfter = productTransaction.QuantityAfter,
					TransactionDate = productTransaction.TransactionDate,
					DoneBy = productTransaction.DoneBy,
					UnitPrice = productTransaction.UnitPrice
				};

			// Return the search results.
			return results;
		}
	}
}
