using System;
using System.Collections.Generic;
using System.Text;
using IMS.CoreBusiness;
using IMS.UseCases.Activities.Interfaces;
using IMS.UseCases.PluginInterfaces;

namespace IMS.UseCases.Activities
{
	public class ProduceProductUseCase : IProduceProductUseCase
	{
		private readonly IProductTransactionRepository _productTransactionRepository;
		private readonly IProductRepository _productRepository;

		public ProduceProductUseCase(IProductTransactionRepository productTransactionRepository,
			IProductRepository productRepository)
		{
			_productTransactionRepository = productTransactionRepository;
			_productRepository = productRepository;
		}

		public async Task ExecuteAsync(string productionNumber, Product product, int quantity, string producedBy)
		{
			// Add a new transaction record
			await _productTransactionRepository.ProduceAsync(productionNumber, product, quantity, producedBy);

			// Update the quantity of the product
			product.Quantity += quantity;
			await _productRepository.UpdateProductAsync(product);
		}
	}
}
