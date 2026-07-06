using IMS.CoreBusiness;

namespace IMS.UseCases.Inventories.Interfaces;

public interface IGetInventoryByIdUseCase
{
	Task<Inventory> ExecuteAsync(int inventoryID);
}