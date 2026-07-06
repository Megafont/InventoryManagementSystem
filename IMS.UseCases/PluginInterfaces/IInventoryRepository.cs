using IMS.CoreBusiness;

namespace IMS.UseCases.PluginInterfaces;

public interface IInventoryRepository
{
	Task AddInventoryAsync(Inventory inventory);
	Task<IEnumerable<Inventory>> GetInventoriesByNameAsync(string name);
	Task<Inventory?> GetInventoryByIdAsync(int inventoryId);
	Task UpdateInventoryAsync(Inventory inventory);
}