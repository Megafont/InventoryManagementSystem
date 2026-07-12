using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IMS.CoreBusiness.Validations
{
	public class Product_EnsurePriceIsGreaterThanInventoriesCost : ValidationAttribute
	{
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			Product product = validationContext.ObjectInstance as Product;
			if (product != null)
			{
				if (!ValidatePricing(product))
					return new ValidationResult($"The product's price is less than its total cost of its parts ({CalculateTotalInventoriesCost(product).ToString("c")})!",
						new List<string>() { validationContext.MemberName });
			}

			return ValidationResult.Success;
		}

		private decimal CalculateTotalInventoriesCost(Product product)
		{
			if (product == null || product.ProductInventories == null)
				return 0;

			return product.ProductInventories.Sum(x => x.Inventory?.Price * x.InventoryQuantity ?? 0);
		}

		private bool ValidatePricing(Product product)
		{
			if (product == null || product.ProductInventories == null)
				return true;

			return CalculateTotalInventoriesCost(product) <= product.Price;
		}
	}
}
