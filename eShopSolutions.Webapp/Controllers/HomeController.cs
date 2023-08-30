using eShopSolutions.ApiIntergration;
using eShopSolutions.Utilities.Contains;
using eShopSolutions.Webapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolutions.Webapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISlideApiClient _slideApiClient;
        private readonly IProductApiClient _productApiClient;

        public HomeController(ILogger<HomeController> logger, ISlideApiClient slideApiClient, IProductApiClient productApiClient )
        {
            _logger = logger;
            _slideApiClient = slideApiClient;
            _productApiClient = productApiClient;   
        }

        public async Task<IActionResult> Index()
        {
            var languageId = "vi";
           /* var culture = CultureInfo.CurrentCulture.Name;*/
            var viewModels = new HomeViewModels()
            {
                Slides = await _slideApiClient.GetAll(),
                FeaturedProducts = await _productApiClient.GetFeaturedProducts(languageId, SystemConstants.ProductsSetting.NumberOfFeaturedProducts),
            };
            return View(viewModels);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
