using eShopSolutions.ViewModels.Catalog.Category;
using eShopSolutions.ViewModels.Catalog.Products;
using eShopSolutions.ViewModels.Common.Dtos;
using eShopSolutions.ViewModels.System.Users;
using eShopSolutions.ViewModels.Utilities.SlideViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolutions.Application.Utilities
{
    public interface ISlidesServices
    {
        public Task<List<SlideViewModels>> GetAll( );
      
    }
}
