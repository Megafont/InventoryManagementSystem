using IMS.CoreBusiness;

namespace IMS.UseCases.Products.Interfaces;

public interface IGetProductByIdUseCase
{
	Task<Product?> ExecuteAsync(int productID);
}