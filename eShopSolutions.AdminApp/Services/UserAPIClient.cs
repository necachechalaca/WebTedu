using eShopSolutions.ViewModels.Common.Dtos;
using eShopSolutions.ViewModels.System.Users;
using Microsoft.AspNetCore.Http;
using Nest;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolutions.AdminApp.Services
{
    public class UserAPIClient : IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory; // dung de goi webapi ~~ httpclient
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserAPIClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ApiResult<string>> Authenticate(LoginRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httcontent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");



            var response = await client.PostAsync("/api/users/authenticate", httcontent);
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<string>>(await response.Content.ReadAsStringAsync());
            }



            return JsonConvert.DeserializeObject<ApiErorResult<string>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ApiResult<UserViewModel>> GetUserId(Guid id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer" + _httpContextAccessor.HttpContext.Session.GetString("Token"));

            var responese = await client.GetAsync($"/api/users/{id}");
            var body = await responese.Content.ReadAsStringAsync();
            if(responese.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<UserViewModel>> (body);

            }
            return JsonConvert.DeserializeObject<ApiErorResult<UserViewModel>>(body);
        }

        public async Task<ApiResult<PageResult<UserViewModel>>> GetUserPaging(GetUserPaginfRequest request)
        {

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer" + _httpContextAccessor.HttpContext.Session.GetString("Token"));

            var response = await client.GetAsync($"/api/users/paging?pageIndex= " +
                $"{request.PageIndex}&pageSize={request.PageSize}&keywork = {request.KeyWord}");
            var token = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<ApiResult<PageResult<UserViewModel>>>(token);
            return user;
        }

        public async Task<ApiResult<bool>> RegisterUser(RegisterRequest registerRequest)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");

            var json = JsonConvert.SerializeObject(registerRequest);
            var httcontent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/users/register", httcontent);

            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);

            return JsonConvert.DeserializeObject<ApiErorResult<bool>>(result);

        }

        public async Task<ApiResult<bool>> Edit(Guid id,UserUpdateRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
                ("Bearer" + _httpContextAccessor.HttpContext.Session.GetString("Token"));


            var json = JsonConvert.SerializeObject(request);

            var httpcontent = new StringContent(json,  Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"/api/users/{id}", httpcontent);

            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);

            return JsonConvert.DeserializeObject<ApiErorResult<bool>>(result);
        }

        public async Task<ApiResult<bool>> Delete(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client  = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",sessions);

            var response = await client.DeleteAsync($"/api/users/{id}");

            var body =  await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(body);
            }
            return JsonConvert.DeserializeObject<ApiErorResult<bool>>(body);

        }
    }
}
