using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using IMS.WebApp.ViewModels;

namespace IMS.CoreBusiness.Validations
{
	/// <summary>
	/// This validator checks that there is enough inventory available for all parts needed to produce this product.
	/// </summary>
	/// <remarks>>
	/// IMPORTANT:
	///		If you see this validator allowing you to produce as much of a product as you like, make sure the product has its inventories specified.
	///		Otherwise, there is nothing for the validator to check since the product has no inventory requirements. Thus, no error will appear.
	/// </remarks>
	public class ProduceProduct_EnsureEnoughInventoryQuantity : ValidationAttribute
	{
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			var produceProductViewModel = validationContext.ObjectInstance as ProduceProductViewModel;
			if (produceProductViewModel != null)
			{
				if (produceProductViewModel.Product != null)
				{
					foreach (ProductInventory productInventory in produceProductViewModel.Product.ProductInventories)
					{
						if (productInventory.Inventory != null && 
						    productInventory.InventoryQuantity * produceProductViewModel.QuantityToProduce > productInventory.Inventory.Quantity)
						{
							return new ValidationResult(
								$"The inventory ({productInventory.Inventory.InventoryName}) is insufficient to produce {produceProductViewModel.QuantityToProduce} units of product ({produceProductViewModel.Product.ProductName}).",
								new [] { validationContext.MemberName });
						}
					} // end foreach
				}
			}

			return ValidationResult.Success;
		}


	}
}
