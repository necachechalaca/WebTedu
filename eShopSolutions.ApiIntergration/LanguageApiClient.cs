using eShopSolutions.ApiIntergration;
using eShopSolutions.ViewModels.Common.Dtos;
using eShopSolutions.ViewModels.System.Roles;
using eShopSolutions.ViewModels.System.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace eShopSolutions.ApiIntergration
{
    public class LanguageApiClient : ILanguageApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory; // dung de goi webapi ~~ httpclient
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LanguageApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ApiResult<List<LanguageVm>>> GetALL()
        {
            var secssion = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer" + secssion);

            var responese = await client.GetAsync($"/api/languages");
            var body = await responese.Content.ReadAsStringAsync();
            if (responese.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<List<LanguageVm>>>(body);

                

            }
            return JsonConvert.DeserializeObject<ApiErorResult<List<LanguageVm>>>(body);
        }
    }
}
