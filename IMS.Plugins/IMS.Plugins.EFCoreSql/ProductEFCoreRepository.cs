using System;
using System.Collections.Generic;
using System.Text;
using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugins.EFCoreSqlServer
{
	public class ProductEFCoreRepository : IProductRepository
	{
		private readonly IDbContextFactory<IMS_Db_Context> _contextFactory;

		public ProductEFCoreRepository(IDbContextFactory<IMS_Db_Context> contextFactory)
		{
			_contextFactory = contextFactory;
		}

		public async Task AddProductAsync(Product product)
		{
			var db = _contextFactory.CreateDbContext();

			db.Products?.Add(product);

			FlagInventoriesUnchanged(product, db);

			await db.SaveChangesAsync();
		}

		public async Task DeleteProductByIdAsync(int productID)
		{
			using (IMS_Db_Context db = _contextFactory.CreateDbContext())
			{
				var product = await db.Products.FindAsync(productID);
				if (product == null)
					return;


				db.Products?.Remove(product);
				await db.SaveChangesAsync();
			}
		}

		public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
		{
			// This is the same as a using block, except that it means this variable will be disposed when it goes out of scope (aka when this function ends).
			using IMS_Db_Context db = _contextFactory.CreateDbContext();

			return await db.Products.Where(x => x.ProductName.ToLower().IndexOf(name.ToLower()) >= 0).ToListAsync();
		}

		public async Task<Product?> GetProductByIdAsync(int productID)
		{
			// This is the same as a using block, except that it means this variable will be disposed when it goes out of scope (aka when this function ends).
			using IMS_Db_Context db = _contextFactory.CreateDbContext();

			Product product = await db.Products.FindAsync(productID);

			return product;
		}

		public async Task UpdateProductAsync(Product product)
		{
			// This is the same as a using block, except that it means this variable will be disposed when it goes out of scope (aka when this function ends).
			using IMS_Db_Context db = _contextFactory.CreateDbContext();

			Product record = await db.Products.FindAsync(product.ProductID);
			if (record != null)
			{
				record.ProductName = product.ProductName;
				record.Price = product.Price;
				record.Quantity = product.Quantity;
				record.ProductInventories = product.ProductInventories;

				FlagInventoriesUnchanged(product, db);

				await db.SaveChangesAsync();
			}
		}

		/// <summary>
		/// This method marks the product inventories inside a Product object as unchanged.
		/// Doing this ensures that the Inventory table is not modified when we update a product
		/// in the Products table.
		///
		/// The course also said we need to do this, as otherwise Entity Framework Core will add them as
		/// new entries in the Inventories table when a product is added into the database, rather than
		/// using the existing ones.
		/// </summary>
		/// <param name="product">The product object whose inventories should be marked unchanged.</param>
		/// <param name="db">Thee database context.</param>
		private void FlagInventoriesUnchanged(Product product, IMS_Db_Context db)
		{
			if (product?.ProductInventories != null &&
			    product.ProductInventories.Count > 0)
			{
				foreach (ProductInventory prodInv in product.ProductInventories)
				{
					if (prodInv.Inventory != null)
					{
						db.Entry(prodInv.Inventory).State = EntityState.Unchanged;
					}
				} // end foreach
			}
		}
	}
}
