using IMS.CoreBusiness;

namespace IMS.UseCases.Inventories.Interfaces;

public interface IDeleteInventoryByIdUseCase
{
	Task ExecuteAsync(int inventoryID);
}