using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using IMS.UseCases.PluginInterfaces;
using IMS.WebApp.ViewModels;

namespace IMS.CoreBusiness.Validations
{
	/// <summary>
	/// This validator checks that the production number is unique (not already used by another record in the repository).
	/// </summary>
	public class ProduceProduct_EnsureProductionNumberIsUnique : ValidationAttribute
	{
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			
			var produceProductViewModel = validationContext.ObjectInstance as ProduceProductViewModel;
			var productTransactionRepository = validationContext.GetService<IProductTransactionRepository>();

			if (produceProductViewModel != null)
			{
				if (!string.IsNullOrWhiteSpace(produceProductViewModel.ProductionNumber))
				{
					ProductTransaction transaction = productTransactionRepository.GetProductTransactionByProductionNumberAsync(
							produceProductViewModel.ProductionNumber).Result;

					if (transaction != null)
					{
						// We found a transaction with the specified production number, so return an error message.
						return new ValidationResult(
							$"The production # ({produceProductViewModel.ProductionNumber}) is already in use by another product transaction in the database!",
							new[] { validationContext.MemberName });
					}
				}

			}

			return ValidationResult.Success;
		}


	}
}
