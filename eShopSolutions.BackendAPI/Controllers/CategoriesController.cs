using eShopSolutions.Application.Utilities;
using eShopSolutions.ViewModels.Catalog.Category;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShopSolutions.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : Controller
    {
        private readonly ISlidesServices _categorysServices;
        public CategoriesController(ISlidesServices categorysServices)
        {
            _categorysServices = categorysServices; 
        }
        [HttpGet]
        public async Task<IActionResult> GetAll( )
        {
            var categories = await _categorysServices.GetAll();
            return Ok(categories);
        }
        
    }
}
