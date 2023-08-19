using eShopSolution.Database.Entity;
using eShopSolutions.Utilities.Exeptions;
using eShopSolutions.ViewModels.Common.Dtos;
using eShopSolutions.ViewModels.System.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolutions.Application.System.Users
{
    public class UserSevices : IUserServices
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _configuration;
        public UserSevices(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IConfiguration configuration) 
        {
            _userManager = userManager;
            _signManager = signInManager;   
            _roleManager = roleManager; 
            _configuration = configuration;
        }

        public async Task<string> Authencate(LoginRequest request) // dang nhap
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
             throw new eShopExceptions("Tài khoản không tồn tại");
            }
            var result = await _signManager.PasswordSignInAsync(user, request.Password, false, false);
            if (!result.Succeeded)
            {
                throw new eShopExceptions("Đăng nhập không đúng");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new[]
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName,user.FirstName),
                new Claim(ClaimTypes.Role, string.Join(";",roles)),
                new Claim(ClaimTypes.Name, request.UserName)
            };
            var secetKey = new SymmetricSecurityKey// class abstract doi xung  de xac thuc signature
                (Encoding.UTF8.GetBytes(_configuration["Tokens:Key"])); // dung de ma hoa du lieu  
            var creds = new SigningCredentials(secetKey, SecurityAlgorithms.HmacSha256);
            // SigningCredentials dai dien cho khoa mat ma va bao mat
            // SecurityAlgorithms xac dinh cho cac dai dien thuat toan ma hoa
            // HmacSha256 trỏ đến thuật toán mật mã 256,  truong nay la hang so

            var token = new JwtSecurityToken  //SecurityToken được thiết kế để đại diện cho Mã thông báo Web JSON (JWT).
            (_configuration["Tokens:Issuer"], _configuration["Tokens:Issuer"],
            claims, expires:DateTime.Now.AddHours(2),signingCredentials: creds);
            //  trong JwtSecurityToken gom 3 tp , Payload, header, RawSignature
            // Payload nội dung thông tin mà người dùng mong muốn bên trong chuỗi JSON

            return new  JwtSecurityTokenHandler().WriteToken(token);
            //SecurityTokenHandler được thiết kế để tạo và xác thực Json Web Tokens


        }

        public async Task<PageResult<UserViewModel>> GetUserPaging(GetUserPaginfRequest request)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(request.KeyWord))
            {
                query = query.Where(x=> x.UserName.Contains( request.KeyWord) 
                || x.PhoneNumber.Contains(request.KeyWord));
            }
            int totalRaw = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)

                .Select(x => new UserViewModel()
                {
                    Email = x.Email,    
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    PhoneNumber = x.PhoneNumber,
                    UserName = x.UserName,
                    Id = x.Id,
                }).ToListAsync() ;

            var pagedresult = new PageResult<UserViewModel>()
            {
                TotalRecord = totalRaw,
                Items = data
            };
            return pagedresult;
        }

        public async Task<bool> Register(RegisterRequest request) // dang ky
        {
            var user = new AppUser()
            {
                UserName = request.UserName,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
              
            };
            var result = await _userManager.CreateAsync(user, request.Password);    
            if(result.Succeeded)
            {
                return true;
            }
            return false;
        }
    }
}
