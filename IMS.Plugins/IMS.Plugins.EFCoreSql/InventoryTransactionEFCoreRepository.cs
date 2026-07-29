using System;
using System.Collections.Generic;
using System.Text;
using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugins.EFCoreSqlServer
{
	public class InventoryTransactionEFCoreRepository : IInventoryTransactionRepository
	{
		private readonly IDbContextFactory<IMS_Db_Context> _contextFactory;


		public InventoryTransactionEFCoreRepository(IDbContextFactory<IMS_Db_Context> contextFactory)
		{
			_contextFactory = contextFactory;
		}

		public async Task PurchaseAsync(string poNumber, Inventory inventory, int quantity, string purchasedBy, decimal price)
		{
			using IMS_Db_Context db = _contextFactory.CreateDbContext();

			db.InventoryTransactions?.Add(new InventoryTransaction()
			{
				PurchaseOrderNumber = poNumber,
				InventoryID = inventory.InventoryID,
				QuantityBefore = inventory.Quantity,
				ActivityType = InventoryTransactionTypes.PurchaseInventory,
				QuantityAfter = inventory.Quantity + quantity,
				TransactionDate = DateTime.Now, // In real life this would be DateTime.UtcNow in a company that spans multiple time zones.
				DoneBy = purchasedBy,
				UnitPrice = price,
			});

			await db.SaveChangesAsync();
		}

		public async Task ProduceAsync(string productionNumber, Inventory inventory, int quantityToConsume, string producedBy, decimal? price)
		{
			using IMS_Db_Context db = _contextFactory.CreateDbContext();

			db.InventoryTransactions?.Add(new InventoryTransaction
			{
				ProductionNumber = productionNumber,
				InventoryID = inventory.InventoryID,
				QuantityBefore = inventory.Quantity,
				ActivityType = InventoryTransactionTypes.ProduceProduct,
				QuantityAfter = inventory.Quantity - quantityToConsume,
				TransactionDate = DateTime.Now, // In real life this would be DateTime.UtcNow in a company that spans multiple time zones.
				DoneBy = producedBy,
				UnitPrice = price
			});

			await db.SaveChangesAsync();
		}

		public async Task<IEnumerable<InventoryTransaction>> GetInventoryTransactionsAsync(string inventoryName, DateTime? dateFrom, DateTime? dateTo,
			InventoryTransactionTypes? transactionType)
		{
			using IMS_Db_Context db = _contextFactory.CreateDbContext();

			// Use linq to filter the inventory transactions based on the passed in filtering options.
			var results = from inventoryTransaction in db.InventoryTransactions
				join inventory in db.Inventories on inventoryTransaction.InventoryID equals inventory.InventoryID
				where
					(string.IsNullOrWhiteSpace(inventoryName) ||
					 inventory.InventoryName.ToLower().IndexOf(inventoryName.ToLower()) >= 0) &&
					(!dateFrom.HasValue || inventoryTransaction.TransactionDate >= dateFrom.Value.Date) &&
					(!dateTo.HasValue || inventoryTransaction.TransactionDate <= dateTo.Value.Date) &&
					(!transactionType.HasValue || inventoryTransaction.ActivityType == transactionType)
				select inventoryTransaction;

			// Return the search results.
			// The Include() call is telling Entity Framework Core that we want the returned inventory transactions
			// to include the Inventory records they link to.
			return await results.Include(x => x.Inventory).ToListAsync();
		}
	}
}
