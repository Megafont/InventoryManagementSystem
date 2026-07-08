using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
	}
}
