using System.Threading.Tasks;
using eShopSolutions.Utilities.Contains;
using eShopSolutions.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using eShopSolutions.ViewModels.Catalog.Products.Public;
using eShopSolutions.ApiIntergration;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nest;
using eShopSolutions.ViewModels.System.Users;
using System;
using eShopSolutions.ViewModels.Common.Dtos;

namespace eShopSolution.AdminApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IConfiguration _configuration;
        private readonly ICategoryApiClient _categoryApiClient;

        public ProductController(IProductApiClient productApiClient,
            IConfiguration configuration,
            ICategoryApiClient categoryApiClient)
        {
            _configuration = configuration;
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
        }

        public async Task<IActionResult> Index(string keyword, int? categoryId, int pageIndex = 1, int pageSize = 10)
        {
            var languageId = "vi";

            var request = new GetProductPagingRequest()
            {
               
                PageIndex = pageIndex,
                PageSize = pageSize,
                KeyWord = keyword,
                LanguageId = languageId,
                CategoryId = categoryId
            };
            var data = await _productApiClient.GetPagings(request);
            ViewBag.KeyWord = keyword;
            var catigories = await _categoryApiClient.GetAll(languageId);
            ViewBag.Categories = catigories.Select(x=> new SelectListItem()
            {
                Text  = x.Name,
                Value = x.Id.ToString(),
            });
           
                


            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _productApiClient.CreateProduct(request);
            if (result)
            {
                TempData["result"] = "Thêm mới sản phẩm thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Thêm sản phẩm thất bại");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> CategoryAssign(int id)
        {
            var categoriesAssignRequest = await GetCategoryAssignRequest(id);

            return View(categoriesAssignRequest);
        }


        [HttpPost]
        public async Task<IActionResult> CategoryAssign(CategoryAssignRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            } 
            var result = await _productApiClient.CategoryAssign(request.Id ,request);

            if (result.IsSuccess)
            {
                TempData["result"] = " Cap nhat thanh cong";
                return RedirectToAction("Index");   
            }
                
            ModelState.AddModelError("", result.Message);
            var rolesAssignRequest = GetCategoryAssignRequest(request.Id);
            return View(rolesAssignRequest);
        }
        private async Task<CategoryAssignRequest> GetCategoryAssignRequest(int id)
        {
            var languageId = "vi";

            var productObj = await _productApiClient.GetById(id, languageId);
            var categories = await _categoryApiClient.GetAll(languageId);
            var categoriesAssignRequest = new CategoryAssignRequest();
            foreach (var role in categories)
            {
                categoriesAssignRequest.Categories.Add(new SelectItem()
                {
                    Id = role.Id.ToString(),
                    Name = role.Name,
                    Selected = productObj.Categories.Contains(role.Name)
                });
            }
            return categoriesAssignRequest;

        }
    }
}
