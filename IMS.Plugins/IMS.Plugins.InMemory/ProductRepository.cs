using System.Runtime.CompilerServices;
using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory
{
	public class ProductRepository : IProductRepository
	{
		private readonly IInventoryRepository _inventoryRepository;
		private List<Product> _products;

		public ProductRepository(IInventoryRepository inventoryRepository)
		{
			_inventoryRepository = inventoryRepository;

			_products = new List<Product>()
			{
				new Product { ProductID = 1, ProductName = "Bike", Quantity = 10, Price = 150 },
				new Product { ProductID = 2, ProductName = "Car", Quantity = 10, Price = 25000 },
			};
		}


		public Task AddProductAsync(Product product)
		{
			if (_products.Any(x =>
				    x.ProductName.Equals(product.ProductName, StringComparison.OrdinalIgnoreCase)))
			{
				return Task.CompletedTask;
			}


			// Create an idea for the new product.
			int maxID = _products.Max(x => x.ProductID);
			product.ProductID = maxID + 1;

			_products.Add(product);

			return Task.CompletedTask;
		}

		public Task DeleteProductByIdAsync(int productID)
		{
			Product Product = _products.FirstOrDefault(x => x.ProductID == productID);

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

		public async Task<Product?> GetProductByIdAsync(int productId)
		{
			Product product = _products.FirstOrDefault(x => x.ProductID == productId);

			// Make a copy of the product object before returning it to stop outside code being able to modify the product object in the repository directly. This won't be necessary once we're using a real database.
			Product? copy = null;
			if (product != null)
			{
				copy = new Product();

				copy.ProductID = product.ProductID;
				copy.ProductName = product.ProductName;
				copy.Price = product.Price;
				copy.Quantity = product.Quantity;

				copy.ProductInventories = new List<ProductInventory>();
				if (product.ProductInventories != null && product.ProductInventories.Count > 0)
				{
					foreach (var productInventory in product.ProductInventories)
					{
						ProductInventory productInventoryCopy = new ProductInventory
						{
							InventoryID = productInventory.InventoryID,
							ProductID = productInventory.ProductID,
							Product = product,
							Inventory = new Inventory(),
							InventoryQuantity = productInventory.InventoryQuantity,
						};

						if (productInventory.Inventory != null)
						{
							// Get the inventory data straight from the InventoryRepository so we have the most up-to-date data.
							// This fixes a bug when using the in-memory repositories where you can produce more product than you have inventory for.
							Inventory currentInv =
								await _inventoryRepository.GetInventoryByIdAsync(productInventory.Inventory.InventoryID);

							if (currentInv != null)
							{
								productInventoryCopy.Inventory.InventoryID = currentInv.InventoryID;
								productInventoryCopy.Inventory.InventoryName = currentInv.InventoryName;
								productInventoryCopy.Inventory.Price = currentInv.Price;
								productInventoryCopy.Inventory.Quantity = currentInv.Quantity;
							}
						}
						copy.ProductInventories.Add(productInventoryCopy);
					}
				}
			}

			return await Task.FromResult(copy);
		}

		public Task UpdateProductAsync(Product product)
		{
			// Make sure the updated product data does not have the same name as another product in the repository.
			if (_products.Any(x => 
				    x.ProductID != product.ProductID &&
			        x.ProductName.Equals(product.ProductName, StringComparison.OrdinalIgnoreCase)))
			{
				return Task.CompletedTask;
			}

			var productToUpdate = _products.FirstOrDefault(x => x.ProductID == product.ProductID);

			if (productToUpdate is not null)
			{
				// Can use Automapper here instead of copying all properties manually.
				// The course said this is definitely better when you have 10+ properties to copy here.
				productToUpdate.ProductName = product.ProductName;
				productToUpdate.Quantity = product.Quantity;
				productToUpdate.Price = product.Price;
				productToUpdate.ProductInventories = product.ProductInventories;
			}

			return Task.CompletedTask;
		}

	}
}
