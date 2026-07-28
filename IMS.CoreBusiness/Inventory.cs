using IMS.CoreBusiness.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

// NOTE: This project is called CoreBusiness, but it represents the Entities layer at the center of the clean architecture (aka onion architecture) diagram.

namespace IMS.CoreBusiness
{
	public class Inventory
	{
		public int InventoryID { get; set; }
		[Required, StringLength(150)]
		public string InventoryName { get; set; } = string.Empty;
		[Range(0, int.MaxValue, ErrorMessage="Quantity must be greater than or equal to 0!")]
		public int Quantity { get; set; }
		[Range(0, int.MaxValue, ErrorMessage = "Price must be greater than or equal to 0!")]
		public decimal Price { get; set; }

		// This is called a navigation property in database lingo.
		// This is a custom validation attribute we made to ensure that the product's price is greater than the total cost of its parts.
		[Product_EnsurePriceIsGreaterThanInventoriesCost]
		public List<ProductInventory> ProductInventories { get; set; } = new();
	}
}
