using IMS.CoreBusiness;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugins.EFCoreSqlServer
{
	public class IMS_Db_Context : DbContext
	{
		public DbSet<Inventory>? Inventories { get; set; }
		public DbSet<Product>? Products { get; set; }
		public DbSet<ProductInventory>? ProductInventories { get; set; }
		public DbSet<InventoryTransaction>? InventoryTransactions { get; set; }
		public DbSet<ProductTransaction>? ProductTransactions { get; set; }


		public IMS_Db_Context(DbContextOptions<IMS_Db_Context> options)
		: base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			/* This is how you set the precision of a field to get rid of warnings in the console while the app is running.
			   There might be an attribute you can use to do this in the model classes, such as Inventory.cs or Product.cs, too.

			modelBuilder.Entity<Inventory>()
				.Property(i => i.Price).HasPrecision(18, 4);

			modelBuilder.Entity<Product>()
				.Property(p => p.Price).HasPrecision(18, 4);

			modelBuilder.Entity<InventoryTransaction>()
				.Property(it => it.UnitPrice).HasPrecision(18, 4);

			modelBuilder.Entity<ProductTransaction>()
				.Property(pt => pt.UnitPrice).HasPrecision(18, 4);
			*/

			modelBuilder.Entity<ProductInventory>()
				.HasKey(pi => new { pi.ProductID, pi.InventoryID });

			modelBuilder.Entity<ProductInventory>()
				.HasOne(pi => pi.Product)
				.WithMany(p => p.ProductInventories)
				.HasForeignKey(pi => pi.ProductID);

			modelBuilder.Entity<ProductInventory>()
				.HasOne(pi => pi.Inventory)
				.WithMany(i => i.ProductInventories)
				.HasForeignKey(pi => pi.InventoryID);


			// Seed data.
			modelBuilder.Entity<Inventory>().HasData(
				new Inventory { InventoryID = 1, InventoryName = "Bike Seat", Quantity = 10, Price = 2 },
				new Inventory { InventoryID = 2, InventoryName = "Bike Body", Quantity = 10, Price = 15 },
				new Inventory { InventoryID = 3, InventoryName = "Bike Wheels", Quantity = 20, Price = 8 },
				new Inventory { InventoryID = 4, InventoryName = "Bike Pedals", Quantity = 20, Price = 1 },
				new Inventory { InventoryID = 5, InventoryName = "Gas Engine", Quantity = 50, Price = 1500 },
				new Inventory { InventoryID = 6, InventoryName = "Electric Engine", Quantity = 30, Price = 4000 }
			);

			modelBuilder.Entity<Product>().HasData(
				new Product { ProductID = 1, ProductName = "Bike", Quantity = 10, Price = 150 },
				new Product { ProductID = 2, ProductName = "Car", Quantity = 5, Price = 25000 },
				new Product { ProductID = 3, ProductName = "Electric Car", Quantity = 5, Price = 40000 }
			);

			modelBuilder.Entity<ProductInventory>().HasData(
				new ProductInventory { ProductID = 1, InventoryID = 1, InventoryQuantity = 1 }, // bike seat
				new ProductInventory { ProductID = 1, InventoryID = 2, InventoryQuantity = 1 }, // bike body
				new ProductInventory { ProductID = 1, InventoryID = 3, InventoryQuantity = 2 }, // bike wheels
				new ProductInventory { ProductID = 1, InventoryID = 4, InventoryQuantity = 2 },  // bike pedals

				new ProductInventory { ProductID = 2, InventoryID = 5, InventoryQuantity = 1 },  // gas engine

				new ProductInventory { ProductID = 3, InventoryID = 6, InventoryQuantity = 1 }   // electric engine
			);
		}
	}
}
