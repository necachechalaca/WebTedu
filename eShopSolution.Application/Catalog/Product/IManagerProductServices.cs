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
    public interface IManagerProductServices
    {
        Task<int> Create(ProductCreateRequest request);

        Task<int> Update(ProductUpdateRequest request);
        Task<int> Delete(int productId);
         
        Task<bool> UpdatePrice(int  productId,decimal newPrice);  
        Task<bool> UpdateStock(int  productId, int  addQuantity);  
        
        Task AddViewCount(int productId);

       Task< List<ProductViewModel>> GetAll();
     

        Task<PageResult<ProductViewModel>> GetAllPaging(GetProductPagingRequest request);
    }
}
