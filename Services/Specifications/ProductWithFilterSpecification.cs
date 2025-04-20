using Domain.Contract;
using Domain.Entities;
using Shared.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class ProductWithFilterSpecification : Specification<Product>
    {
        public ProductWithFilterSpecification(int id)
            : base(product => product.Id == id)
        {
            AddInclude(product => product.ProductBrand);
            AddInclude(product => product.ProductType);
        }

        public ProductWithFilterSpecification(ProductSpecificationParams specs)
           : base(product => (!specs.BrandId.HasValue || product.BrandId == specs.BrandId) &&
                             (!specs.TypeId.HasValue || product.TypeId == specs.TypeId) &&
                             (string.IsNullOrWhiteSpace(specs.Search) ||
                             product.Name.ToLower().Contains(specs.Search.ToLower().Trim()))
           )
        {

            AddInclude(product => product.ProductBrand);
            AddInclude(product => product.ProductType);

            ApplyPagination(specs.PageIndex, specs.PageSize);

            if (specs.Sort is not null)
            {
                switch (specs.Sort)
                {
                    case SortingOptions.NameAsc:
                        SetOrderBy(product => product.Name);
                        break;
                    case SortingOptions.NameDesc:
                        SetOrderByDescending(product => product.Name);
                        break;
                    case SortingOptions.PriceAsc:
                        SetOrderBy(product => product.Price);
                        break;
                    case SortingOptions.PriceDesc:
                        SetOrderByDescending(product => product.Price);
                        break;
                }
            }
        }


    }
}
