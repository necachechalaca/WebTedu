
using eShopSolutions.Database.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eShopSolutions.Database.Entity;

using eShopSolutions.Utilities.Exeptions;
using Microsoft.EntityFrameworkCore;

using eShopSolutions.ViewModels.Catalog.Products.Manager;
using eShopSolutions.ViewModels.Common.Dtos;
using eShopSolutions.ViewModels.Catalog.Products;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using eShopSolutions.Application.Common;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Data.SqlClient;

namespace eShopSolutions.Application.Catalog.Product
{
    public class ManagerProductServices : IManagerProductServices
    {
        private readonly EShopDbContext _context;
        private readonly FileStorageService _fileStorageService;
        public ManagerProductServices(EShopDbContext eShopDbContext, FileStorageService fileStorageService) 
        {
            _context = eShopDbContext;  
            _fileStorageService = fileStorageService;
        }

        public async Task AddViewCount(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            product.ViewCount += 1;
           await _context.SaveChangesAsync();    
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new Productt()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ViewCount = 0,
                DateCreated = DateTime.Now,
                ProductTranslations = new List<ProductTranslation> { new ProductTranslation()
                {
                    Name = request.Name,    
                    Description = request.Description,
                    Details = request.Details,
                    SeoAlias = request.SeoAlias,    
                    SeoDescription = request.SeoDescription,    
                    SeoTitle = request.SeoTitle,    
                    LanguageId = request.LanguageId,
                } }
            };
            // save image
            if(request.ThumpnailImage != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        Caption = "Thumbnail image",
                        DateCreated = DateTime.Now,
                        FileSize =  request.ThumpnailImage.Length,
                        ImagePath = await this.SaveFile(request.ThumpnailImage),
                        IsDefault = true,
                        SortOder = 1
                        

                    }
                };
            }
            _context.Products.Add(product);

         return  await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int productId)
        {
           var product = _context.Products.Find(productId);
            if (product == null) throw new eShopExceptions($"Can't find product: {productId}");

            var images = _context.ProductImages
                   .Where(i => i.IsDefault == true && i.ProductId == productId);
                   
           foreach ( var image in images) 
            {

              await  _fileStorageService.DeleteFileAsync(image.ImagePath);
            }

            _context.Products.Remove(product);

            return await _context.SaveChangesAsync();
        }

        public Task<List<ProductViewModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<PageResult<ProductViewModel>> GetAllPaging(GetProductPagingRequest request)
        {
            // SELEC
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId 
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id 
                        select new { p, pt, pic }; 
            // filter
            if(!string.IsNullOrEmpty(request.KeyWord)) 
                query = query.Where(x=>x.pt.Name.Contains(request.KeyWord));  
            
            if(request.CategoryId == null && request.CategoryId != 0)
            {
                query = query.Where(p =>p.pic.CategoryId == request.CategoryId);
            }
            // Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
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
            // selec end project

            var pageResult = new PageResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                Items = data
            };
            return pageResult;  
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);
            var productTransition  = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == request.Id && x.LanguageId == request.LanguageId);  
            if(product == null || productTransition == null) throw new eShopExceptions($"Can't find product: {request.Id}");

            productTransition.Name = request.Name;
            productTransition.Description = request.Description;
            productTransition.Details = request.Details;
            productTransition.SeoAlias = request.SeoAlias;  
            productTransition.SeoDescription = request.SeoDescription;  
            productTransition.SeoTitle = request.SeoTitle;
            productTransition.Details = request.Details;
            if (request.ThumpnailImage != null)
            {
                var thumpnailImage = await _context.ProductImages
                    .Where(i=>i.IsDefault == true && i.ProductId  == request.Id)
                    .FirstOrDefaultAsync();
                if (thumpnailImage != null)
                {


                    thumpnailImage.FileSize = request.ThumpnailImage.Length;
                    thumpnailImage.ImagePath = await this.SaveFile(request.ThumpnailImage);
                  _context.ProductImages.Update(thumpnailImage);    
                }
              
            }
            return await  _context.SaveChangesAsync();
        }

        public async Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            var productt = await _context.Products.FindAsync(productId);
            if (productt == null) throw new eShopExceptions($"Can't find product : {productId}");
            productt.Price = newPrice; 
            return await _context.SaveChangesAsync() >0;  
        }

        public async Task<bool> UpdateStock(int productId, int addQuantity)
        {
            var product = await _context.Products.FindAsync();
            if (product == null) throw new eShopExceptions($"Can't find product : {productId}");
            product.Stock = addQuantity;
            return  await _context.SaveChangesAsync() > 0;
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var filename = $"{Guid.NewGuid()}.{Path.GetExtension(originalFileName)}";
            await _fileStorageService.SaveFileAsync(file.OpenReadStream(), filename);
            return filename;
        }
    }
}
