using IMS.CoreBusiness;

namespace IMS.UseCases.Products.Interfaces;

public interface IGetProductsByNameUseCase
{
	Task<IEnumerable<Product>> ExecuteAsync(string name = "");
}