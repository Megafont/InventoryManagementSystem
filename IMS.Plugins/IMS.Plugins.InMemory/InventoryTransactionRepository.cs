using System;
using System.Collections.Generic;
using System.Text;
using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory
{
	public class InventoryTransactionRepository : IInventoryTransactionRepository
	{
		public List<InventoryTransaction> _inventoryTransactions = new();

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

		public Task ProduceAsync(string productionNumber, Inventory inventory, int quantityToConsume, string producedBy, decimal price)
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
		}
	}
}
