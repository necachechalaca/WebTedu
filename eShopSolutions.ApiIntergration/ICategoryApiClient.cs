using eShopSolutions.ViewModels.Catalog.Category;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShopSolutions.ApiIntergration
{
    public interface ICategoryApiClient
    {
        public Task<List<CategoryViewModels>> GetAll(string languageId);

    }
}
