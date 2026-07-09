using System;
using System.Collections.Generic;
using System.Text;
using IMS.CoreBusiness;
using IMS.UseCases.Products.Interfaces;
using IMS.UseCases.PluginInterfaces;

namespace IMS.UseCases.Products
{
	public class AddProductUseCase : IAddProductUseCase
	{
		private readonly IProductRepository _productRepository;

		public AddProductUseCase(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public async Task ExecuteAsync(Product product)
		{
			await _productRepository.AddProductAsync(product);
		}
	}
}
