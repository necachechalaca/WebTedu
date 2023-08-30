using eShopSolutions.ViewModels.Catalog.Products;
using eShopSolutions.ViewModels.Common;
using eShopSolutions.ViewModels.System.Users;
using eShopSolutions.Database.Entity;
using eShopSolutions.ViewModels.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolutions.ViewModels.Catalog.Products.Public;

namespace eShopSolutions.ApiIntergration
{
    public interface IProductApiClient
    {
        Task<PageResult<ProductViewModel>> GetPagings(GetProductPagingRequest request);

        Task<bool> CreateProduct(ProductCreateRequest request);

        Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request);
        Task<ProductViewModel> GetById(int id, string languageId);

        Task<List<ProductViewModel>> GetFeaturedProducts( string languageId, int take);

    }
}