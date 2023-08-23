
using eShopSolutions.Database.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eShopSolutions.Database.Entity;

using eShopSolutions.Utilities.Exeptions;
using Microsoft.EntityFrameworkCore;
using eShopSolutions.ViewModels.Catalog.Products;
using eShopSolutions.ViewModels.Common.Dtos;
using eShopSolutions.ViewModels.Catalog;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using eShopSolutions.Application.Common;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Data.SqlClient;
using eShopSolutions.ViewModels.Catalog.Products.Public;
using eShopSolutions.ViewModels.Catalog.ProductImage;
using static System.Net.Mime.MediaTypeNames;

namespace eShopSolutions.Application.Catalog.Product
{
    public class ManagerProductServices : IManagerProductServices
    {
        private readonly EShopDbContext _context;
        private readonly IStorageService _fileStorageService;
        public ManagerProductServices(EShopDbContext eShopDbContext, IStorageService fileStorageService) 
        {
            _context = eShopDbContext;  
            _fileStorageService = fileStorageService;
        }

        public async Task<int> AddImage(int productId, ProductImageCreateRequest request)
        {

            var productImage = new ProductImage()
            {
                Caption = request.Caption,  
                DateCreated = DateTime.UtcNow,
                IsDefault = request.IsDefault,
                ProductId = productId,
                SortOder = request.SortOrder,   

            };
            if(request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;


            }
            _context.ProductImages.Add(productImage);   
           await _context.SaveChangesAsync();
            return productImage.Id;

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

           await _context.SaveChangesAsync();
            return product.Id;
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

        public async Task<int> DeleteImage(int imageId)
        {
           var images = await _context.ProductImages.FindAsync(imageId);
            if (images == null)
            {
                throw new eShopExceptions("Can't find");
            }
            _context.ProductImages.Remove(images);
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
            
            if(request.CategoryId.Count()>0)
            {
                query = query.Where(p => request.CategoryId.Contains(p.pic.CategoryId));
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
                TotalRecords = totalRow,
                Items = data
            };
            return pageResult;  
        }

        public async Task<ProductViewModel> GetById(int productId, string languageId)
        {
            var product = await _context.Products.FindAsync(productId);
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == productId
            && x.LanguageId == languageId);

            var categories = await (from c in _context.Categories
                                    join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                                    join pic in _context.ProductInCategories on c.Id equals pic.CategoryId
                                    where pic.ProductId == productId && ct.LanguageId == languageId
                                    select ct.Name).ToListAsync();

            var image = await _context.ProductImages.Where(x => x.ProductId == productId && x.IsDefault == true).FirstOrDefaultAsync();

            var productViewModel = new ProductViewModel()
            {
                Id = product.Id,
                DateCreated = product.DateCreated,
                Description = productTranslation != null ? productTranslation.Description : null,
                LanguageId = productTranslation.LanguageId,
                Details = productTranslation != null ? productTranslation.Details : null,
                Name = productTranslation != null ? productTranslation.Name : null,
                OriginalPrice = product.OriginalPrice,
                Price = product.Price,
                SeoAlias = productTranslation != null ? productTranslation.SeoAlias : null,
                SeoDescription = productTranslation != null ? productTranslation.SeoDescription : null,
                SeoTitle = productTranslation != null ? productTranslation.SeoTitle : null,
                Stock = product.Stock,
                ViewCount = product.ViewCount,
                Categories = categories,
                ThumbnailImage = image != null ? image.ImagePath : "no-image.jpg"
            };
            return productViewModel;
        }

        public Task<ProductViewModel> GetById(int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductImageViewModel> GetImageById(int imageId)
        {
            var images = await _context.ProductImages.FindAsync(imageId);
            if(images == null)
            {
                throw new eShopExceptions("Can't find");
            }
            var viewModel = new ProductImageViewModel()
            {
                Caption = images.Caption,
                DateCreated = images.DateCreated,
                FileSize = images.FileSize,
                Id = images.Id,
                ImagePath = images.ImagePath,
                IsDefault = images.IsDefault,
                ProductId = images.ProductId,
                SortOrder = images.SortOder

            };
            return viewModel;   
        }

        public Task<ProductImageViewModel> GetImageById(int imageId, string languageId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProductImageViewModel>> GetListImage(int productId)
        {
            return await _context.ProductImages.Where(x => x.ProductId == productId)
               .Select(i => new ProductImageViewModel()
               {
                   Caption = i.Caption,
                   DateCreated = i.DateCreated,
                   FileSize = i.FileSize,
                   Id = i.Id,
                   ImagePath = i.ImagePath,
                   IsDefault = i.IsDefault,
                   ProductId = i.ProductId,
                   SortOrder = i.SortOder
               }).ToListAsync();
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

        public async Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if(productImage == null)
            {
                throw new eShopExceptions("Can't find");
            }
            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Update(productImage);
            return await _context.SaveChangesAsync();   


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
