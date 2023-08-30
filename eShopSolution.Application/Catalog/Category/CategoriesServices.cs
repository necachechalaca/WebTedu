using eShopSolutions.Application.Common;
using eShopSolutions.Database.EF;
using eShopSolutions.ViewModels.Catalog.Category;
using eShopSolutions.ViewModels.Catalog.Products;
using eShopSolutions.ViewModels.Common.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolutions.Application.Catalog.Category
{
    public class CategoriesServices : ICategoriesServices
    {
        private readonly EShopDbContext _context;
       
        public CategoriesServices(EShopDbContext eShopDbContext)
        {
            _context = eShopDbContext;

        }

      

        public async Task<List<CategoryViewModels>> GetAll(string languageId)
        {
            var a = new List<CategoryViewModels>();
            try
            {
                var query = from c in _context.Categories
                            join ct in _context.CategoryTranslations on c.Id equals ct.Id
                            where ct.LanguageId == languageId
                            select new { c, ct };
                return await query.Select(x => new CategoryViewModels()
                {
                    Id = x.c.Id,
                    Name = x.ct.Name,
                }).ToListAsync();
            }catch (Exception ex) 
            {
                return a;
            }
          
        }

    }
}
