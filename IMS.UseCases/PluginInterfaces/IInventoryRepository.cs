using IMS.CoreBusiness;

namespace IMS.UseCases.PluginInterfaces;

public interface IInventoryRepository
{
	Task AddInventoryAsync(Inventory inventory);
	Task DeleteInventoryByIdAsync(int inventoryID);
	Task<IEnumerable<Inventory>> GetInventoriesByNameAsync(string name);
	Task<Inventory?> GetInventoryByIdAsync(int inventoryID);
	Task UpdateInventoryAsync(Inventory inventory);
}
