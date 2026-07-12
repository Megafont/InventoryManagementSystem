using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using IMS.CoreBusiness.Validations;

// NOTE: This project is called CoreBusiness, but it represents the Entities layer at the center of the clean architecture (aka onion architecture) diagram.

namespace IMS.CoreBusiness
{
	public class Product
	{
		public int ProductID { get; set; }
		[Required, StringLength(150)]
		public string ProductName { get; set; } = string.Empty;
		[Range(0, int.MaxValue, ErrorMessage="Quantity must be greater than or equal to 0!")]
		public int Quantity { get; set; }
		[Range(0, int.MaxValue, ErrorMessage = "Price must be greater than or equal to 0!")]
		public decimal Price { get; set; }

		// This is a custom validation attribute we made to ensure that the product's price is greater than the total cost of its parts.
		[Product_EnsurePriceIsGreaterThanInventoriesCost]
		public List<ProductInventory> ProductInventories { get; set; } = new();


		public void AddInventory(Inventory inventory)
		{
			// Check if this product already has an entry for this inventory type.
			if (!ProductInventories.Any(x =>
				    x.Inventory is not null && 
			        x.Inventory.InventoryID == inventory.InventoryID))
			{

				ProductInventories.Add(new ProductInventory
				{
					InventoryID = inventory.InventoryID,
					Inventory = inventory,
					InventoryQuantity = 1,
					ProductID = ProductID,
					Product = this
				});
			}
		}

		public void RemoveInventory(ProductInventory productInventory)
		{
			ProductInventories?.Remove(productInventory);
		}

	}
}
