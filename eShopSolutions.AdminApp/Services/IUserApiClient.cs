using eShopSolutions.ViewModels.Common.Dtos;
using eShopSolutions.ViewModels.System.Users;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace eShopSolutions.AdminApp.Services
{
    public interface IUserApiClient
    {
        Task<string> Authenticate(LoginRequest request);

        Task<PageResult<UserViewModel>>GetUserPaging (GetUserPaginfRequest request);
    }
}
