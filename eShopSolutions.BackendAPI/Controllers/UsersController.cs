using eShopSolutions.Application.System.Users;
using eShopSolutions.ViewModels.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;


namespace eShopSolutions.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        
        private readonly IUserServices _userServices;
        public UsersController(IUserServices userServices) 
        { 
            _userServices = userServices;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            var resultToken = await _userServices.Authencate(request);

            if(string.IsNullOrEmpty(resultToken.ResultObj))
            { 
                return BadRequest(resultToken);
            }
           
            return Ok ( resultToken );
               
        }
        [HttpPost("register")]                                                                                      
      
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
        
            var result = await _userServices.Register(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);

        }
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery]GetUserPaginfRequest request)
        {
           var product = await _userServices.GetUserPaging(request);    
            return Ok(product);
        }




        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userServices.Edit(id,request);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userServices.GetUserByID(id);
            return Ok(user);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userServices.Delete(id);
            return Ok(result);
        }

    }
}
