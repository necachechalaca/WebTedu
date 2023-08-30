
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
using Nest;
using eShopSolutions.Utilities.Contains;
using Microsoft.AspNetCore.Identity;

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
            int result = 0;
            try
            {
                var languages = _context.Languages;
                var translations = new List<ProductTranslation>();
                foreach (var language in languages)
                {
                    if (language.Id == request.LanguageId)
                    {
                        translations.Add(new ProductTranslation()
                        {
                            Name = request.Name,
                            Description = request.Description,
                            Details = request.Details,
                            SeoDescription = request.SeoDescription,
                            SeoAlias = request.SeoAlias,
                            SeoTitle = request.SeoTitle,
                            LanguageId = request.LanguageId
                        });
                    }
                    else
                    {
                        translations.Add(new ProductTranslation()
                        {
                            Name = "N/A",
                            Description = "N/A",
                            SeoAlias = "N/A",
                            LanguageId = language.Id
                        });
                    }
                }
                var product = new Productt()
                {
                    Price = request.Price,
                    OriginalPrice = request.OriginalPrice,
                    Stock = request.Stock,
                    ViewCount = 0,
                    DateCreated = DateTime.Now,
                    ProductTranslations = translations
                };
                //Save image
                if (request.ThumpnailImage != null)
                {
                    product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        Caption = "Thumbnail image",
                        DateCreated = DateTime.Now,
                        ImagePath = await this.SaveFile(request.ThumpnailImage),
                        IsDefault = true,
                        SortOder = 1
                    }
                };
                }
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                result = product.Id;

            }
            catch (Exception ex) 
            { 
            }
            return result;
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
            var result = new PageResult<ProductViewModel>();
            try
            {
                var query = from p in _context.Products
                            join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                            join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                            from pic in ppic.DefaultIfEmpty()
                            join c in _context.Categories on pic.CategoryId equals c.Id into picc
                            from c in picc.DefaultIfEmpty()
                            where pt.LanguageId == request.LanguageId
                            select new { p, pt, pic};
                //2. filter
                if (!string.IsNullOrEmpty(request.KeyWord))
                    query = query.Where(x => x.pt.Name.Contains(request.KeyWord));
                if (request.CategoryId != null && request.CategoryId != 0)
                {
                    query = query.Where(p => p.pic.CategoryId == request.CategoryId);
                }


                //3. Pagingb
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
                        ViewCount = x.p.ViewCount,
                        
                    }).ToListAsync();

                //4. Select and projection
                var pagedResult = new PageResult<ProductViewModel>()
                {
                    TotalRecords = totalRow,
                    PageSize = request.PageSize,
                    PageIndex = request.PageIndex,
                    Items = data
                };
                return pagedResult;
            }catch (Exception ex)
            {
                return result;
            }    
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
                Categories = categories
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
        public async Task<ApiResult<bool>> CategoryAssign(int Id, CategoryAssignRequest request)
        {
            var user = await _context.Products.FindAsync(Id);
            if (user == null)
            {
                return new ApiErorResult<bool>( $"Sản phẩm với {Id} không tồn tại");
            }

          
            foreach (var category in request.Categories)
            {
                var productInCategory = await _context.ProductInCategories
                    .FirstOrDefaultAsync(x => x.CategoryId == int.Parse(category.Id) && x.ProductId == Id);

                if (productInCategory != null && category.Selected ==false)
                {
                     _context.ProductInCategories.Remove(productInCategory);
                }
                else if (productInCategory == null && category.Selected)
                {
                    await _context.ProductInCategories.AddAsync(new ProductInCategory()
                    {
                        CategoryId = int.Parse( category.Id),
                        ProductId = Id
                    });
                }
            }
           

            await  _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();

        }

        public async Task<List<ProductViewModel>> GetFeaturedProducts(string languageId, int take)
        {
            var result = new List<ProductViewModel>();
            try
            {  //1. Select join
                var query = from p in _context.Products
                            join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                            join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                            //join pi in _context.ProductImages.Where(x => x.IsDefault == true) on p.Id equals pi.ProductId
                            from pic in ppic.DefaultIfEmpty()
                            join c in _context.Categories on pic.CategoryId equals c.Id into picc
                            from c in picc.DefaultIfEmpty()
                            where pt.LanguageId == languageId
                            select new { p, pt, pic };

                var data = await query.OrderByDescending(x => x.p.DateCreated).Take(take)
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
                        ViewCount = x.p.ViewCount,
                        //ThumbnailImage = x.pi.ImagePath
                    }).ToListAsync();

                

                return data;
            }
            catch (Exception ex)
            {
                return result;
            }
        }
    }
}
