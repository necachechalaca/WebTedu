using eShopSolutions.ViewModels.Common.Dtos;
using eShopSolutions.ViewModels.System.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolutions.ApiIntergration
{
    public interface ILanguageApiClient
    {
        public Task<ApiResult<List<LanguageVm>>> GetALL();
    }
}
