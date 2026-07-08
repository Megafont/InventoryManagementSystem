using System.Runtime.CompilerServices;
using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory
{
	public class ProductRepository : IProductRepository
	{
		private List<Product> _products;

		public ProductRepository()
		{
			_products = new List<Product>()
			{
				new Product { ProductID = 1, ProductName = "Bike", Quantity = 10, Price = 100 },
				new Product { ProductID = 2, ProductName = "Car", Quantity = 10, Price = 25000 },
			};
		}


		public Task AddProductAsync(Product Product)
		{
			if (_products.Any(x =>
				    x.ProductName.Equals(Product.ProductName, StringComparison.OrdinalIgnoreCase)))
			{
				return Task.CompletedTask;
			}


			// Create an idea for the new Product.
			int maxID = _products.Max(x => x.ProductID);
			Product.ProductID = maxID + 1;

			_products.Add(Product);

			return Task.CompletedTask;
		}

		public Task DeleteProductByIdAsync(int ProductID)
		{
			Product Product = _products.FirstOrDefault(x => x.ProductID == ProductID);

			if (Product != null)
				_products.Remove(Product);

			return Task.CompletedTask;
		}

		public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
				return await Task.FromResult(_products);

			return _products.Where(x => x.ProductName.Contains(name, StringComparison.OrdinalIgnoreCase));
		}

		public async Task<Product?> GetProductByIdAsync(int ProductId)
		{
			return await Task.FromResult(_products.FirstOrDefault(x => x.ProductID == ProductId));
		}

		public Task UpdateProductAsync(Product Product)
		{
			if (_products.Any(x => x.ProductID != Product.ProductID &&
			                     x.ProductName.Equals(Product.ProductName, StringComparison.OrdinalIgnoreCase)))
			{
				return Task.CompletedTask;
			}

			var ProductToUpdate = _products.FirstOrDefault(x => x.ProductID == Product.ProductID);

			if (ProductToUpdate is not null)
			{
				// Can use Automapper here instead of copying all properties manually.
				// The course said this is definitely better when you have 10+ properties to copy here.
				ProductToUpdate.ProductName = Product.ProductName;
				ProductToUpdate.Quantity = Product.Quantity;
				ProductToUpdate.Price = Product.Price;
			}

			return Task.CompletedTask;
		}

	}
}
