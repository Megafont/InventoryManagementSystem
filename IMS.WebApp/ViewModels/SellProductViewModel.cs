using System.ComponentModel.DataAnnotations;
using IMS.CoreBusiness;
using IMS.CoreBusiness.Validations;

namespace IMS.WebApp.ViewModels
{
	public class SellProductViewModel
	{
		[Required]
		public string SalesOrderNumber { get; set; } = string.Empty;

		[Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "You have to select a product.")]
		public int ProductID { get; set; }

		[Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Quantity has to be greater than or equal to 1.")]
		[SellProduct_EnsureEnoughProductQuantity]
		public int QuantityToSell { get; set; }

		[Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "Price has to be greater than or equal to 0.")]
		public decimal UnitPrice { get; set; }

		public Product? Product { get; set; }

	}
}
