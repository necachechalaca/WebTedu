
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eShopSolutions.ViewModels.System.Users;

using eShopSolutions.ViewModels.Common.Dtos;

namespace eShopSolutions.Application.System.Users
{
    public interface IUserServices
    {
        Task<string> Authencate(LoginRequest request);
        Task<bool> Register(RegisterRequest request);
        Task<PageResult<UserViewModel>> GetUserPaging(GetUserPaginfRequest request);
    }
}
