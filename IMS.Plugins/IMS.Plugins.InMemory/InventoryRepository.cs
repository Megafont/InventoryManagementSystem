using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory
{
	public class InventoryRepository : IInventoryRepository
	{
		private List<Inventory> _inventories;

		public InventoryRepository()
		{
			_inventories = new List<Inventory>()
			{
				new Inventory { InventoryID = 1, InventoryName = "Bike Seat", Quantity = 10, Price = 2 },
				new Inventory { InventoryID = 2, InventoryName = "Bike Body", Quantity = 10, Price = 15 },
				new Inventory { InventoryID = 3, InventoryName = "Bike Wheels", Quantity = 20, Price = 8 },
				new Inventory { InventoryID = 4, InventoryName = "Bike Pedals", Quantity = 20, Price = 1 }
			};
		}

		public async Task<IEnumerable<Inventory>> GetInventoriesByNameAsync(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
				return await Task.FromResult(_inventories);

			return _inventories.Where(x => x.InventoryName.Contains(name, StringComparison.OrdinalIgnoreCase));
		}

		public Task AddInventoryAsync(Inventory inventory)
		{
			if (_inventories.Any(x =>
				    x.InventoryName.Equals(inventory.InventoryName, StringComparison.OrdinalIgnoreCase)))
			{
				return Task.CompletedTask;
			}


			// Create an idea for the new inventory.
			int maxID = _inventories.Max(x => x.InventoryID);
			inventory.InventoryID = maxID + 1;

			_inventories.Add(inventory);

			return Task.CompletedTask;
		}
	}
}
