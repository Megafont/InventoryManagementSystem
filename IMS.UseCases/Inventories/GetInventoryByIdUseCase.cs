using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using IMS.CoreBusiness;
using IMS.UseCases.Inventories.Interfaces;
using IMS.UseCases.PluginInterfaces;

namespace IMS.UseCases.Inventories
{
	public class GetInventoryByIdUseCase : IGetInventoryByIdUseCase
	{
		private readonly IInventoryRepository _inventoryRepository;

		public GetInventoryByIdUseCase(IInventoryRepository inventoryRepository)
		{
			_inventoryRepository = inventoryRepository;
		}

		public async Task<Inventory> ExecuteAsync(int inventoryID)
		{
			return await _inventoryRepository.GetInventoryByIdAsync(inventoryID);
		}
	}
}
