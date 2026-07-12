using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace IMS.CoreBusiness
{
	public class ProductInventory
	{
		public int ProductID { get; set; }

		[JsonIgnore]
		public Product? Product { get; set; }
		

		public int InventoryID { get; set; }

		[JsonIgnore]
		public Inventory? Inventory { get; set; }


		public int InventoryQuantity { get; set; }

		public decimal TotalCost => Inventory?.Price * InventoryQuantity ?? 0;
	}
}
