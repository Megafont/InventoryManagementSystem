using System;
using System.Collections.Generic;
using System.Text;
using IMS.CoreBusiness;

namespace IMS.UseCases.PluginInterfaces
{
	public interface IProductRepository
	{
		Task AddProductAsync(Product product);
		Task DeleteProductByIdAsync(int productID);
		Task<IEnumerable<Product>> GetProductsByNameAsync(string name);
		Task<Product?> GetProductByIdAsync(int productId);
		Task UpdateProductAsync(Product product);
	}
}
