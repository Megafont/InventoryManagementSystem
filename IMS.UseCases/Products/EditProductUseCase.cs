using IMS.CoreBusiness;
using IMS.UseCases.Products.Interfaces;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMS.UseCases.Products
{
	public class EditProductUseCase : IEditProductUseCase
	{
		private readonly IProductRepository _productRepository;

		public EditProductUseCase(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public async Task ExecuteAsync(Product product)
		{
			await _productRepository.UpdateProductAsync(product);
		}
	}
}
