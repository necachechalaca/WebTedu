using eShopSolutions.ViewModels.Catalog.Products;
using eShopSolutions.ViewModels.Common.Dtos;
using eShopSolutions.ViewModels.Catalog;   
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolutions.Application.Catalog.Product
{
    public interface IPublicProductServices
    {
       Task < PageResult<ProductViewModel>> GetAllByCategories(GetPublicProductPagingRequest  request);


        Task<List<ProductViewModel>> GetAll();
    }
}
