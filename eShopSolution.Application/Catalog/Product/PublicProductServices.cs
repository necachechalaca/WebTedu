using eShopSolutions.ViewModels.Catalog.Products.Manager;
using eShopSolutions.ViewModels.Common.Dtos;
using eShopSolutions.ViewModels.Catalog.Products;
using eShopSolutions.Database.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolutions.Application.Catalog.Product
{
    public class PublicProductServices :  IPublicProductServices
    {
        private readonly EShopDbContext _context;

        public PublicProductServices(EShopDbContext context)
        {
            this._context = context;
        }

        public async Task<PageResult<ProductViewModel>> GetAllByCategories(GetProductPagingRequest request)
        {
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        select new { p, pt, pic };
            if(request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                 query = query.Where(p=> p.pic.CategoryId == request.CategoryId);   
            }
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount
                }).ToListAsync();
            var pageResult = new PageResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                Items = data
            };
            return pageResult;
            
        }
    }
}
