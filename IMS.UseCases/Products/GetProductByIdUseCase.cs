using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using IMS.CoreBusiness;
using IMS.UseCases.Inventories.Interfaces;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Products.Interfaces;

namespace IMS.UseCases.Inventories
{
	public class GetProductByIdUseCase : IGetProductByIdUseCase
	{
		private readonly IProductRepository _productRepository;

		public GetProductByIdUseCase(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public async Task<Product?> ExecuteAsync(int productID)
		{
			return await _productRepository.GetProductByIdAsync(productID);
		}
	}
}
