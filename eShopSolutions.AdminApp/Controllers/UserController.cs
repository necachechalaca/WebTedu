using eShopSolutions.ApiIntergration;
using eShopSolutions.Utilities.Contains;
using eShopSolutions.ViewModels.Common.Dtos;
using eShopSolutions.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Nest;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolutions.AdminApp.Controllers
{
   
    public class UserController : Controller
    {
        /* private readonly IConfiguration _configuration;*/
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration; //private readonly chi goi 1 lan
        private readonly IRolesApiClient _rolesApiClient;
        public UserController(IUserApiClient userApiClient, IConfiguration configuration, IRolesApiClient rolesApiClient)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
            _rolesApiClient = rolesApiClient;
        }
        public async Task<IActionResult>Index(string keyword, int pageIndex =1, int  pageSize =10)
        {
          

            var request = new GetUserPaginfRequest()
            {

                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await  _userApiClient.GetUserPaging(request);
            /* ViewBag.Keyword = keyword;*/

            return View(data.ResultObj);
        }


        [HttpGet]
        public async Task< IActionResult> Login()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var token = await _userApiClient.Authenticate(request);

            var userPrincipal = this.ValidateToken(token.ResultObj);
            var authProperties = new AuthenticationProperties // lưu trữ trạng thái và phiên xác thực
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMonths(10), // lấy hoặc gán thời gian tokent hết hạn
                IsPersistent = true // ghi nhớ đăng nhập
            };
            HttpContext.Session.SetString(SystemConstants.AppSettings.DefaultLanguageId, _configuration["DefaultLanguageId"]);
            HttpContext.Session.SetString(SystemConstants.AppSettings.Token, token.ResultObj); // cậu đẩy cái này vào cookie nhé
            // đăng nhập
            await HttpContext.SignInAsync(
                       CookieAuthenticationDefaults.AuthenticationScheme,
                       userPrincipal,
                       authProperties);
            

            return RedirectToAction("Index", "Home");
           

           
        }
        [HttpPost]       
        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");    
            return RedirectToAction("Login", "User");

        }
        // ham giai ma token
        // token được mã hóa gửi đến  ClaimsPrincipal để giải mã chứa thông tin đăng nhập vd như các claim 
        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true; //   cho biết liệu PII có được hiển thị trong nhật ký hay không. Fail theo mặc định.

            SecurityToken validatedToken; // đại diện cho lớp cơ sở được triển khai các mã thông báo bảo mật
            TokenValidationParameters validationParameters = new TokenValidationParameters(); // chứa tập hợp tham số sử dụng khi xác thực SecurityToken

            validationParameters.ValidateLifetime = true; //để kiểm soát xem thời gian tồn tại có được xác thực trong quá trình xác thực mã thông báo hay không.

            validationParameters.ValidAudience = _configuration["Tokens:Issuer"]; // lấy hoặc kiểm tra đối tượng có được xác thực trong quá trình xác thực mã
            validationParameters.ValidIssuer = _configuration["Tokens:Issuer"]; // lấy hoặc kiểm tra issuer có được xác thực trong quá trình xác thực
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"])); // giải mã ra thành key
            //IssuerSigningKey lấy hoặc kiểm tra xem  SecurityToken có được gọi

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);
            // JwtSecurityTokenHandler tạo và xác thực web tokent
            // ValidateToken đọc và xác thực
            return principal;

          //  principal chứa giá trị cliam đã gắn cho token

            

        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task< IActionResult> Create(RegisterRequest request)
        {
            if(!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _userApiClient.RegisterUser(request);
            if (result.IsSuccess)
                return RedirectToAction("Index");
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public async Task< IActionResult> Edit(Guid id)
        {
            var result = await _userApiClient.GetUserId(id);
            if(result.IsSuccess)
            {
                var user = result.ResultObj;
                var userUpdate = new UserUpdateRequest()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Id = id
                };
                return View(userUpdate);    
            }
            
            return RedirectToAction("Eror", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Edit( UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _userApiClient.Edit(request.Id, request);
            if (result.IsSuccess)
                return RedirectToAction("Index");
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _userApiClient.GetUserId(id);
            return View(result.ResultObj);    
        }


        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            return View(new UserDeleteRequest()
            {
                Id = id
            }); ;
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UserDeleteRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _userApiClient.Delete(request.Id);
            if (result.IsSuccess)
                return RedirectToAction("Index");
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> RolesAssign(Guid id)
        {
            var rolesAssignRequest = await GetRolesAssignRequest(id);




            return View(rolesAssignRequest);
        }
      
        
        [HttpPost]
        public async Task<IActionResult> RolesAssign(RolesAssignRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _userApiClient.RolesAssign(request.Id, request);
            if (result.IsSuccess)
                return RedirectToAction("Index");
            ModelState.AddModelError("", result.Message);
            var rolesAssignRequest = GetRolesAssignRequest(request.Id);
            return View(rolesAssignRequest);
        }

        private async Task< RolesAssignRequest> GetRolesAssignRequest(Guid id) 
        {
            var result = await _userApiClient.GetUserId(id);
            var roleObj = await _rolesApiClient.GetAll();
            var rolesAssignRequest = new RolesAssignRequest();
            foreach (var role in roleObj.ResultObj)
            {
                rolesAssignRequest.Roles.Add(new SelectItem()
                {
                    Id = role.Id.ToString(),
                    Name = role.Name,
                    Selected = result.ResultObj.Roles.Contains(role.Name)
                });
            }
            return rolesAssignRequest;

        }


    }
}
