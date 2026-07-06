using IMS.CoreBusiness;

namespace IMS.UseCases.Inventories.Interfaces;

public interface IGetInventoriesByNameUseCase
{
	Task<IEnumerable<Inventory>> ExecuteAsync(string name = "");
}