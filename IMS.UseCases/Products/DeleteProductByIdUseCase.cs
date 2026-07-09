using IMS.CoreBusiness;
using IMS.UseCases.Inventories.Interfaces;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Products.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMS.UseCases.Inventories
{
	public class DeleteProductByIdUseCase : IDeleteProductByIdUseCase
	{
		private readonly IProductRepository _productRepository;

		public DeleteProductByIdUseCase(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public async Task ExecuteAsync(int productID)
		{
			await _productRepository.DeleteProductByIdAsync(productID);
		}
	}
}
