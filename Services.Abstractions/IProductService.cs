using Shared;
using Shared.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IProductService
    {
        Task<PaginatedResult<ProductResultDto>> GetAllProductsAsync(ProductSpecificationParams specifications);

        Task<ProductResultDto> GetProductByIdAsync(int id);

        Task<IEnumerable<TypeResultDto>> GetAllTypesAsync();

        Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync();
    }
}
