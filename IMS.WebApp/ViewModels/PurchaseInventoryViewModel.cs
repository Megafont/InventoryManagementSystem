using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModels
{
	public class PurchaseInventoryViewModel
	{
		[Required]
		public string PurchaseOrderNumber { get; set; } = string.Empty;

		// This range starts at 1, as this confirms that the user has selected an inventory. Otherwise this value would be 0.
		[Range(1, int.MaxValue, ErrorMessage = "You have to select an inventory.")]
		public int InventoryID { get; set; }

		[Range(1, int.MaxValue, ErrorMessage = "Quantity has to be greater than or equal to 1.")]
		public int QuantityToPurchase { get; set; }

		[Range(0, int.MaxValue, ErrorMessage = "Price has to be greater than or equal to 0.")]
		public decimal InventoryPrice { get; set; }
	}
}
