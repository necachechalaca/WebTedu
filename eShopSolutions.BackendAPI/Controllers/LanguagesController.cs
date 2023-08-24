using eShopSolutions.Application.Catalog.Product;
using eShopSolutions.Application.System.Language;
using eShopSolutions.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eShopSolutions.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguagesController : ControllerBase
    {
        private readonly ILanguageServices _languageServices;

        public LanguagesController(ILanguageServices languageServices)
        {
            _languageServices = languageServices;
        }



        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var product = await _languageServices.GetAll();
            return Ok(product);
        }



    }
}
