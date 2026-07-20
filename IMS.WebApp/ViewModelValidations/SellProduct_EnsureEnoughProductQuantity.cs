using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using IMS.WebApp.ViewModels;

namespace IMS.CoreBusiness.Validations
{
	public class SellProduct_EnsureEnoughProductQuantity : ValidationAttribute
	{
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			var sellProductViewModel = validationContext.ObjectInstance as SellProductViewModel;
			if (sellProductViewModel != null)
			{
				if (sellProductViewModel.Product != null)
				{
						if (sellProductViewModel.Product.Quantity < sellProductViewModel.QuantityToSell)
						{
							return new ValidationResult(
								$"The product ({sellProductViewModel.Product.ProductName}) has insufficient warehouse stock ({sellProductViewModel.Product.Quantity} units).",
								new [] { validationContext.MemberName });
						}
				}
			}

			return ValidationResult.Success;
		}


	}
}
