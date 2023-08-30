using eShopSolutions.ViewModels.Common.Dtos;
using eShopSolutions.ViewModels.System.Roles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShopSolutions.ApiIntergration
{
    public interface IRolesApiClient
    {
        Task<ApiResult<List<RolesViewModels>>> GetAll();
    }
}
