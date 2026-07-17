using System.ComponentModel.DataAnnotations;
using IMS.CoreBusiness;
using IMS.CoreBusiness.Validations;

namespace IMS.WebApp.ViewModels
{
	public class ProduceProductViewModel
	{
		[Required]
		public string ProductionNumber { get; set; } = string.Empty;

		// This range starts at 1, as this confirms that the user has selected a product. Otherwise, this value would be 0.
		[Required]
		[Range(1, int.MaxValue, ErrorMessage = "You have to select a product.")]
		public int ProductID { get; set; }

		[Required]
		[Range(1, int.MaxValue, ErrorMessage = "Quantity has to be greater than or equal to 1.")]
		[ProduceProduct_EnsureEnoughInventoryQuantity]
		public int QuantityToProduce { get; set; }

		public Product? Product { get; set; } = null;

	}
}
