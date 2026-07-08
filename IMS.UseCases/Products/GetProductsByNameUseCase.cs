using System;
using System.Collections.Generic;
using System.Text;

using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Products.Interfaces;

namespace IMS.UseCases.Products
{
	public class GetProductsByNameUseCase : IGetProductsByNameUseCase
	{
		private readonly IProductRepository _productRepository;

		public GetProductsByNameUseCase(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public async Task<IEnumerable<Product>> ExecuteAsync(string name = "")
		{ 
			return await _productRepository.GetProductsByNameAsync(name);
		}
	}

}
