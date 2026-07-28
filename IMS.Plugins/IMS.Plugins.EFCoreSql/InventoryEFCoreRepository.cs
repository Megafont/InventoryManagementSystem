using System;
using System.Collections.Generic;
using System.Text;
using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugins.EFCoreSqlServer
{
	public class InventoryEFCoreRepository : IInventoryRepository
	{
		private readonly IDbContextFactory<IMS_Db_Context> _contextFactory;


		public InventoryEFCoreRepository(IDbContextFactory<IMS_Db_Context> contextFactory)
		{
			_contextFactory = contextFactory;
		}

		public async Task AddInventoryAsync(Inventory inventory)
		{
			using (IMS_Db_Context db = _contextFactory.CreateDbContext())
			{
				db.Inventories?.Add(inventory);
				await db.SaveChangesAsync();
			}
		}

		public async Task DeleteInventoryByIdAsync(int inventoryID)
		{
			using (IMS_Db_Context db = _contextFactory.CreateDbContext())
			{
				var inventory = await db.Inventories.FindAsync(inventoryID);
				if (inventory == null)
					return;
				
				
				db.Inventories?.Remove(inventory);
				await db.SaveChangesAsync();
			}
		}

		public async Task<IEnumerable<Inventory>> GetInventoriesByNameAsync(string name)
		{
			// This is the same as a using block, except that it means this variable will be disposed when it goes out of scope (aka when this function ends).
			using IMS_Db_Context db = _contextFactory.CreateDbContext();

			return await db.Inventories.Where(x => x.InventoryName.ToLower().IndexOf(name.ToLower()) >= 0).ToListAsync();
		}

		public async Task<Inventory?> GetInventoryByIdAsync(int inventoryID)
		{
			// This is the same as a using block, except that it means this variable will be disposed when it goes out of scope (aka when this function ends).
			using IMS_Db_Context db = _contextFactory.CreateDbContext();

			Inventory inventory = await db.Inventories.FindAsync(inventoryID);

			return inventory;
		}

		public async Task UpdateInventoryAsync(Inventory inventory)
		{
			// This is the same as a using block, except that it means this variable will be disposed when it goes out of scope (aka when this function ends).
			using IMS_Db_Context db = _contextFactory.CreateDbContext();

			Inventory record = await db.Inventories.FindAsync(inventory.InventoryID);
			if (record != null)
			{
				record.InventoryName = inventory.InventoryName;
				record.Price = inventory.Price;
				record.Quantity = inventory.Quantity;

				await db.SaveChangesAsync();
			}
		}
	}
}
