using AutoMapper;
using Domain.Contract;
using Domain.Entities;
using Domain.Exceptions;
using Services.Abstractions;
using Services.Specifications;
using Shared;
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

        public async Task<PaginatedResult<ProductResultDto>> GetAllProductsAsync(ProductSpecificationParams specifications)
        {
            var specs = new ProductWithFilterSpecification(specifications);
            var products = await unitOfWork.GetRepository<Product, int>().GetAllAsync(specs);

            var mappedProducts = mapper.Map<IEnumerable<ProductResultDto>>(products);

            var countSpecs = new ProductCountSpecification(specifications);

            var totalCount = await unitOfWork.GetRepository<Product, int>().CountAsync(countSpecs);

            return new PaginatedResult<ProductResultDto>
                (specifications.PageIndex, specifications.PageSize, totalCount, mappedProducts);
        }

        public async Task<IEnumerable<TypeResultDto>> GetAllTypesAsync()
        {
            var types = await unitOfWork.GetRepository<ProductType, int>().GetAllAsync();

            var mappedTypes = mapper.Map<IEnumerable<TypeResultDto>>(types);

            return mappedTypes;
        }

        public async Task<ProductResultDto> GetProductByIdAsync(int id)
        {
            var specs = new ProductWithFilterSpecification(id);
            var product = await unitOfWork.GetRepository<Product, int>().GetAsync(specs);

            return product is null ? throw new ProductNotFoundException(id) : mapper.Map<ProductResultDto>(product);
        }
    }
}
