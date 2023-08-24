using eShopSolutions.ViewModels.Common.Dtos;
using eShopSolutions.ViewModels.System.Roles;
using eShopSolutions.ViewModels.System.Users;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace eShopSolutions.AdminApp.Services
{
    public class RoleApiClient : IRolesApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory; // dung de goi webapi ~~ httpclient
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RoleApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ApiResult<List<RolesViewModels>>> GetAll()
        {
            var secssion = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer" + secssion);

            var responese = await client.GetAsync($"/api/roles");
            var body = await responese.Content.ReadAsStringAsync();
            if (responese.IsSuccessStatusCode)
            {
                List<RolesViewModels> myDeserializedObjList = (List<RolesViewModels>)JsonConvert.DeserializeObject(body, typeof(List<RolesViewModels>));
                return new ApiSuccessResult<List<RolesViewModels>>(myDeserializedObjList);

            }
            return JsonConvert.DeserializeObject<ApiErorResult<List<RolesViewModels>>>(body);
        }
    }
}
