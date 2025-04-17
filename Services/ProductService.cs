using AutoMapper;
using Domain.Contract;
using Domain.Entities;
using Services.Abstractions;
using Shared.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
    {

        public async Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync()
        {
            var brands = await unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();

            var mappedBrands = mapper.Map<IEnumerable<BrandResultDto>>(brands);

            return mappedBrands;
        }

        public async Task<IEnumerable<ProductResultDto>> GetAllProductsAsync()
        {
            var products = await unitOfWork.GetRepository<Product, int>().GetAllAsync();

            var mappedProducts = mapper.Map<IEnumerable<ProductResultDto>>(products);

            return mappedProducts;
        }

        public async Task<IEnumerable<TypeResultDto>> GetAllTypesAsync()
        {
            var types = await unitOfWork.GetRepository<ProductType, int>().GetAllAsync();

            var mappedTypes = mapper.Map<IEnumerable<TypeResultDto>>(types);

            return mappedTypes;
        }

        public async Task<ProductResultDto> GetProductByIdAsync(int id)
        {
            var product = await unitOfWork.GetRepository<Product, int>().GetAsync(id);

            var mappedProduct = mapper.Map<ProductResultDto>(product);

            return mappedProduct;
        }
    }
}
