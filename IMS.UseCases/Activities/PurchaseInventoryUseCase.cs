using System;
using System.Collections.Generic;
using System.Text;
using IMS.CoreBusiness;
using IMS.UseCases.Activities.Interfaces;
using IMS.UseCases.PluginInterfaces;

namespace IMS.UseCases.Activities
{
	public class PurchaseInventoryUseCase : IPurchaseInventoryUseCase
	{
		private readonly IInventoryTransactionRepository _inventoryTransactionRepository;
		private readonly IInventoryRepository _inventoryRepository;

		public PurchaseInventoryUseCase(
			IInventoryTransactionRepository inventoryTransactionRepository,
			IInventoryRepository inventoryRepository)
		{
			_inventoryTransactionRepository = inventoryTransactionRepository;
			_inventoryRepository = inventoryRepository;
		}

		public async Task ExecuteAsync(string poNumber, Inventory inventory, int quantity, string purchasedBy)
		{
			// Insert a new record in the transactions table
			await _inventoryTransactionRepository.PurchaseAsync(poNumber, inventory, quantity, purchasedBy, inventory.Price);

			// Increase the quantity of the purchased inventory that we have
			inventory.Quantity += quantity;
			await _inventoryRepository.UpdateInventoryAsync(inventory);
		}
	}

}
