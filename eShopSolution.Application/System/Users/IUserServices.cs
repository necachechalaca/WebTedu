
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using eShopSolutions.ViewModels.Common.Dtos;
using eShopSolutions.ViewModels.System.Users;

namespace eShopSolutions.Application.System.Users
{
    public interface IUserServices
    {
        Task<ApiResult<string>> Authencate(LoginRequest request);

        Task<ApiResult<bool>> Register(RegisterRequest request);

        Task<ApiResult<PageResult<UserViewModel>>> GetUserPaging(GetUserPaginfRequest request);

        Task<ApiResult<bool>> Edit(Guid id, UserUpdateRequest request);

        Task<ApiResult<UserViewModel>> GetUserByID(Guid id);
        Task<ApiResult<bool>> Delete(Guid Id);

        Task<ApiResult<bool>> RolesAssign(Guid Id,RolesAssignRequest request);

    }
}
