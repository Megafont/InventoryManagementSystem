using System;
using System.Collections.Generic;
using System.Text;

using IMS.CoreBusiness;
using IMS.UseCases.Inventories.Interfaces;
using IMS.UseCases.PluginInterfaces;

namespace IMS.UseCases.Inventories
{
	public class GetInventoriesByNameUseCase : IGetInventoriesByNameUseCase
	{
		private readonly IInventoryRepository _inventoryRepository;

		public GetInventoriesByNameUseCase(IInventoryRepository inventoryRepository)
		{
			_inventoryRepository = inventoryRepository;
		}

		public async Task<IEnumerable<Inventory>> ExecuteAsync(string name = "")
		{ 
			return await _inventoryRepository.GetInventoriesByNameAsync(name);
		}
	}

}
