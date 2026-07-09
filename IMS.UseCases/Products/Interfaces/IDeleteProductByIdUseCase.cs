namespace IMS.UseCases.Products.Interfaces;

public interface IDeleteProductByIdUseCase
{
	Task ExecuteAsync(int productID);
}