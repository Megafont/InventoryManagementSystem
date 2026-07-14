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

		public void PurchaseAsync(string poNumber, Inventory inventory, int quantity, string purchasedBy, decimal price)
		{
			_inventoryTransactions.Add(new InventoryTransaction()
			{
				PurchaseOrderNumber = poNumber,
				InventoryId = inventory.InventoryID,
				QuantityBefore = inventory.Quantity,
				ActivityType = InventoryTransactionType.PurchaseInventory,
				QuantityAfter = inventory.Quantity + quantity,
				TransactionDate = DateTime.Now, // In real life this would be DateTime.UtcNow in a company that spans multiple time zones.
				PurchasedBy = purchasedBy,
				UnitPrice = price,
			});
		}
	}
}
