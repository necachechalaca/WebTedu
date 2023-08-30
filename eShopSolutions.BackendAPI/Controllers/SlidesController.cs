using eShopSolutions.ApiIntergration;
using eShopSolutions.Application.Utilities;
using eShopSolutions.ViewModels.Catalog.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShopSolutions.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class SlidesController : Controller
    {
        private readonly ISlidesServices _iSlidesServices;
        public SlidesController(ISlidesServices iSlidesServices)
        {
            _iSlidesServices = iSlidesServices; 
        }
        [HttpGet]
   
        public async Task<IActionResult> GetAll()
        {
            var slides = await _iSlidesServices.GetAll();
            return Ok(slides);
        }
        
    }
}
