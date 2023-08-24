using eShopSolution.Database.Entity;
using eShopSolutions.Application.System.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eShopSolutions.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IRolesServices _roleServices;
        public RolesController(IRolesServices roleServices)
        {
            _roleServices = roleServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleServices.GetAll();

            return  Ok(roles);
        }
    }
}
