using eShopSolutions.ViewModels.Catalog.Category;
using eShopSolutions.ViewModels.Common.Dtos;
using eShopSolutions.ViewModels.System.Users;
using eShopSolutions.ViewModels.Utilities.SlideViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace eShopSolutions.ApiIntergration
{
    public class SlideApiClient : ISlideApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public SlideApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)

        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<List<SlideViewModels>> GetAll()
        {
            var client = _httpClientFactory.CreateClient();
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/slides");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<List<SlideViewModels>>(result);

            }
            return JsonConvert.DeserializeObject<List<SlideViewModels>>(result);

        }
    }
}
