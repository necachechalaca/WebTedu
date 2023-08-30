using eShopSolutions.ViewModels.Catalog.Products;
using eShopSolutions.ViewModels.Common.Dtos;
using eShopSolutions.ViewModels.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using eShopSolutions.ViewModels.Catalog.ProductImage;
using eShopSolutions.ViewModels.Catalog.Products.Public;

namespace eShopSolutions.Application.Catalog.Product
{
    public interface IManagerProductServices
    {
        Task<int> Create(ProductCreateRequest request);

        Task<int> Update(ProductUpdateRequest request);
        Task<int> Delete(int productId);

        Task<bool> UpdatePrice(int productId, decimal newPrice);
        Task<bool> UpdateStock(int productId, int addQuantity);

        Task AddViewCount(int productId);

        Task<List<ProductViewModel>> GetAll();


        Task<PageResult<ProductViewModel>> GetAllPaging(GetProductPagingRequest request);


        Task<int> AddImage(int productId, ProductImageCreateRequest request);

        Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request);

        Task<int> DeleteImage(int imageId);

        Task<List<ProductImageViewModel>> GetListImage(int productId);

        Task<ProductImageViewModel> GetImageById(int imageId);
        Task<ProductViewModel> GetById(int productId, string languageId);
        Task<ApiResult<bool>> CategoryAssign(int Id, CategoryAssignRequest request);

        Task<List<ProductViewModel>> GetFeaturedProducts(string languageId, int take);
    }
}
