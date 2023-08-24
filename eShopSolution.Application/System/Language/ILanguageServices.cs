using eShopSolutions.ViewModels.Common.Dtos;
using eShopSolutions.ViewModels.System.Users;
using eShopSolutions.ViewModels.System.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolutions.Application.System.Language
{
    public interface ILanguageServices
    {
        Task<ApiResult<List<LanguageVm>>> GetAll();
    }
}
