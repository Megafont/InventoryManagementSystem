using IMS.CoreBusiness;
using IMS.UseCases.Inventories.Interfaces;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMS.UseCases.Inventories
{
	public class DeleteInventoryByIdUseCase : IDeleteInventoryByIdUseCase
	{
		private readonly IInventoryRepository _inventoryRepository;

		public DeleteInventoryByIdUseCase(IInventoryRepository inventoryRepository)
		{
			_inventoryRepository = inventoryRepository;
		}

		public async Task ExecuteAsync(int inventoryID)
		{
			await _inventoryRepository.DeleteInventoryByIdAsync(inventoryID);
		}
	}
}
