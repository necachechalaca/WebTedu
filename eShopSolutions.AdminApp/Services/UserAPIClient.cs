using eShopSolutions.ViewModels.Common.Dtos;
using eShopSolutions.ViewModels.System.Users;
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
        public UserAPIClient(IHttpClientFactory httpClientFactory) 
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<string> Authenticate(LoginRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httcontent = new StringContent(json,Encoding.UTF8, "application/json");
          var client =  _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");



          var response =   await client.PostAsync("/api/users/authenticate", httcontent);

            var token = await response.Content.ReadAsStringAsync();

            return token;   
        }

        public async Task<PageResult<UserViewModel>> GetUserPaging(GetUserPaginfRequest request)
        {
            
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ( "Bearer" + request.BearerToken );

            var response = await client.GetAsync($"/api/users/paging?pageIndex= " + 
                $"{request.PageIndex}&pageSize={request.PageSize}&keywork = {request.KeyWord}");
            var token = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<PageResult<UserViewModel>>(token);
            return user;
        }
    }
}
