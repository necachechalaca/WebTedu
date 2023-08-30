using eShopSolutions.ViewModels.Catalog.Category;
using eShopSolutions.ViewModels.Utilities.SlideViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShopSolutions.ApiIntergration
{
    public interface ISlideApiClient
    {
        public Task<List<SlideViewModels>> GetAll();

    }
}
