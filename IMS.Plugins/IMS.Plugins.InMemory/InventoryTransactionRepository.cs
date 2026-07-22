using System;
using System.Collections.Generic;
using System.Text;
using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory
{
	public class InventoryTransactionRepository : IInventoryTransactionRepository
	{
		private readonly IInventoryRepository _inventoryRepository;
		public List<InventoryTransaction> _inventoryTransactions = new();


		public InventoryTransactionRepository(IInventoryRepository inventoryRepository)
		{
			_inventoryRepository = inventoryRepository;
		}

		public Task PurchaseAsync(string poNumber, Inventory inventory, int quantity, string purchasedBy, decimal price)
		{
			_inventoryTransactions.Add(new InventoryTransaction()
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

			return Task.CompletedTask;
		}

		public Task ProduceAsync(string productionNumber, Inventory inventory, int quantityToConsume, string producedBy, decimal? price)
		{
			_inventoryTransactions.Add(new InventoryTransaction
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

			return Task.CompletedTask;
		}

		public async Task<IEnumerable<InventoryTransaction>> GetInventoryTransactionsAsync(string inventoryName, DateTime? dateFrom, DateTime? dateTo,
			InventoryTransactionTypes? transactionType)
		{
			// You wouldn't normally grab all the transactions like this, but this is an in-memory repository, so it's ok in this case.
			List<Inventory> inventories = (await _inventoryRepository.GetInventoriesByNameAsync(string.Empty)).ToList();

			// Use linq to filter the inventory transactions based on the passed in filtering options.
			var results = from inventoryTransaction in _inventoryTransactions
				join inventory in inventories on inventoryTransaction.InventoryID equals inventory.InventoryID
				where
					(string.IsNullOrWhiteSpace(inventoryName) || inventory.InventoryName.ToLower().IndexOf(inventoryName.ToLower()) >= 0) &&
					(!dateFrom.HasValue || inventoryTransaction.TransactionDate >= dateFrom.Value.Date) &&
					(!dateTo.HasValue || inventoryTransaction.TransactionDate <= dateTo.Value.Date) &&
					(!transactionType.HasValue || inventoryTransaction.ActivityType == transactionType)
				select new InventoryTransaction
				{
					Inventory = inventory,
					InventoryTransactionID = inventoryTransaction.InventoryTransactionID,
					PurchaseOrderNumber = inventoryTransaction.PurchaseOrderNumber,
					ProductionNumber = inventoryTransaction.ProductionNumber,
					InventoryID = inventoryTransaction.InventoryID,
					QuantityBefore = inventoryTransaction.QuantityBefore,
					ActivityType = inventoryTransaction.ActivityType,
					QuantityAfter = inventoryTransaction.QuantityAfter,
					TransactionDate = inventoryTransaction.TransactionDate,
					DoneBy = inventoryTransaction.DoneBy,
					UnitPrice = inventoryTransaction.UnitPrice
				};

			// Return the search results.
			return results;
		}
	}
}
