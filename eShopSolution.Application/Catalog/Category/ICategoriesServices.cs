using eShopSolutions.ViewModels.Catalog.Category;
using eShopSolutions.ViewModels.Catalog.Products;
using eShopSolutions.ViewModels.Common.Dtos;
using eShopSolutions.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolutions.Application.Catalog.Category
{
    public interface ICategoriesServices
    {
        public Task<List<CategoryViewModels>> GetAll(string languageId);
      
    }
}
