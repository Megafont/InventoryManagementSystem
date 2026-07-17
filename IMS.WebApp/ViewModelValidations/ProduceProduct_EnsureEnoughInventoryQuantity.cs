using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using IMS.WebApp.ViewModels;

namespace IMS.CoreBusiness.Validations
{
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
								$"The inventory ({productInventory.Inventory.InventoryName}) is insufficient to produce {produceProductViewModel.QuantityToProduce} of product ({produceProductViewModel.Product.ProductName}).",
								new [] { validationContext.MemberName });
						}
					} // end foreach
				}
			}

			return ValidationResult.Success;
		}


	}
}
