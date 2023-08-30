using eShopSolutions.Application.Common;
using eShopSolutions.Database.EF;
using eShopSolutions.ViewModels.Catalog.Category;
using eShopSolutions.ViewModels.Catalog.Products;
using eShopSolutions.ViewModels.Common.Dtos;
using eShopSolutions.ViewModels.Utilities.SlideViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolutions.Application.Utilities
{
    public class SlidesServices : ISlidesServices
    {
        private readonly EShopDbContext _context;
       
        public SlidesServices(EShopDbContext eShopDbContext)
        {
            _context = eShopDbContext;

        }

        public async Task<List<SlideViewModels>> GetAll()
        {
            var a = new List<SlideViewModels>();
            try
            {
                var query = from c in _context.Slides.OrderBy(c => c.SortOder)

                            select new { c };
                return await query.Select(x => new SlideViewModels()
                {
                    Id = x.c.Id,
                    Name = x.c.Name,
                    Description = x.c.Description,
                    Url = x.c.Url,
                    Image = x.c.Image,

                }).ToListAsync();
            }
            catch (Exception ex)
            {
                return a;
            }
        }

        
    }
}
