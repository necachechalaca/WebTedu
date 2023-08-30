
using eShopSolutions.Utilities.Contains;
using eShopSolutions.ViewModels.Catalog.Products;
using eShopSolutions.ViewModels.Common;
using eShopSolutions.ApiIntergration;
using eShopSolutions.Database.Entity;

using eShopSolutions.ViewModels.Common.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using eShopSolution.ApiIntegration;
using eShopSolutions.ViewModels.Catalog.Products.Public;
using Newtonsoft.Json;
using System.Security.Policy;
using eShopSolutions.ViewModels.System.Users;
using Nest;
using System.Text;


namespace eShopSolutions.ApiIntergration
{
    public class ProductApiClient :  IProductApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ProductApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> CreateProduct(ProductCreateRequest request)
        {
            var sessions = _httpContextAccessor
                .HttpContext
                .Session
                .GetString(SystemConstants.AppSettings.Token);

            var languageId = "vi";

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var requestContent = new MultipartFormDataContent();

            if (request.ThumpnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumpnailImage.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ThumpnailImage.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "thumbnailImage", request.ThumpnailImage.FileName);
            }

            requestContent.Add(new StringContent(request.Price.ToString()), "price");
            requestContent.Add(new StringContent(request.OriginalPrice.ToString()), "originalPrice");
            requestContent.Add(new StringContent(request.Stock.ToString()), "stock");
            requestContent.Add(new StringContent(request.Name.ToString()), "name");
            requestContent.Add(new StringContent(request.Description.ToString()), "description");

            requestContent.Add(new StringContent(request.Details.ToString()), "details");
            requestContent.Add(new StringContent(request.SeoDescription.ToString()), "seoDescription");
            requestContent.Add(new StringContent(request.SeoTitle.ToString()), "seoTitle");
            requestContent.Add(new StringContent(request.SeoAlias.ToString()), "seoAlias");
            requestContent.Add(new StringContent(languageId), "languageId");

            var response = await client.PostAsync($"/api/product/", requestContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<PageResult<ProductViewModel>> GetPagings(GetProductPagingRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            var sessions = _httpContextAccessor
               .HttpContext
               .Session
               .GetString("Token");

            
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await client.GetAsync($"/api/product/paging?PageIndex={request.PageIndex}" +
                 $"&PageSize={request.PageSize}" +
                 $"&KeyWord={request.KeyWord}" + 
                 $"&LanguageId={request.LanguageId}&CategoryId={request.CategoryId}");
            var body = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<PageResult<ProductViewModel>>(body);
            return user;
        }

        public async Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
                ("Bearer" + _httpContextAccessor.HttpContext.Session.GetString("Token"));


            var json = JsonConvert.SerializeObject(request);

            var httpcontent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"/api/product/{id}/categories", httpcontent);

            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);

            return JsonConvert.DeserializeObject<ApiErorResult<bool>>(result);
        }

    

        public async Task<ProductViewModel> GetById(int id, string languageId)
        {
            var client = _httpClientFactory.CreateClient();
            var secssion = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
                ("Bearer" + secssion);


     
            var response = await client.GetAsync($"/api/product/{id}/{languageId}" );

            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ProductViewModel>(result);

            return JsonConvert.DeserializeObject<ProductViewModel>(result);
        }
        public async Task<List<ProductViewModel>> GetFeaturedProducts(string languageId, int take)
        {
            var client = _httpClientFactory.CreateClient();
            var secssion = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
                ("Bearer" + secssion);



            var response = await client.GetAsync($"/api/product/featured/{languageId}/{take}");

            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<List<ProductViewModel>>(result);

            return JsonConvert.DeserializeObject<List<ProductViewModel>>(result);
        }
    }
}