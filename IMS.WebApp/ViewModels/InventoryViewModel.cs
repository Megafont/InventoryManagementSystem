using IMS.CoreBusiness.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

// NOTE: This project is called CoreBusiness, but it represents the Entities layer at the center of the clean architecture (aka onion architecture) diagram.

namespace IMS.CoreBusiness
{
	/// <summary>
	/// This class was added to fix an infinite recursive error happening when trying to add or edit inventories.
	/// See video "86. Solve Static SSR EditForm Issues" in the Udemy course.
	/// </summary>
	public class InventoryViewModel
	{
		public int InventoryID { get; set; }

		[Required, StringLength(150)]
		public string InventoryName { get; set; } = string.Empty;
		
		[Range(0, int.MaxValue, ErrorMessage="Quantity must be greater than or equal to 0!")]
		public int Quantity { get; set; }
		
		[Range(0, int.MaxValue, ErrorMessage = "Price must be greater than or equal to 0!")]
		public decimal Price { get; set; }

	}
}
