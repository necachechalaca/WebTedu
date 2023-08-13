using eShopSolutions.ViewModels.Catalog.Products.Manager;
using eShopSolutions.ViewModels.Common.Dtos;
using eShopSolutions.ViewModels.Catalog.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolutions.Application.Catalog.Product
{
    public interface IPublicProductServices
    {
       Task < PageResult<ProductViewModel>> GetAllByCategories(GetProductPagingRequest  request);
    }
}
