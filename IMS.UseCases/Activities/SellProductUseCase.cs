using System;
using System.Collections.Generic;
using System.Text;
using IMS.CoreBusiness;
using IMS.UseCases.Activities.Interfaces;
using IMS.UseCases.PluginInterfaces;

namespace IMS.UseCases.Activities
{
	public class SellProductUseCase : ISellProductUseCase
	{
		private readonly IProductTransactionRepository _productTransactionRepository;
		private readonly IProductRepository _productRepository;

		public SellProductUseCase(
			IProductTransactionRepository productTransactionRepository,
			IProductRepository productRepository)
		{
			_productTransactionRepository = productTransactionRepository;
			_productRepository = productRepository;
		}

		public async Task ExecuteAsync(string salesOrderNumber, Product product, int quantity, decimal unitPrice, string soldBy)
		{
			await _productTransactionRepository.SellProductAsync(salesOrderNumber, product, quantity, unitPrice, soldBy);

			product.Quantity -= quantity;
			await _productRepository.UpdateProductAsync(product);
		}
	}
}
