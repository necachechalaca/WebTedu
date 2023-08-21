﻿using eShopSolutions.Application.System.Users;
using eShopSolutions.ViewModels.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

            if(string.IsNullOrEmpty(resultToken))
            { 
                return BadRequest("UserName or Password is incorrect.");
            }
           
            return Ok ( resultToken );
               
        }
        [HttpPost("register")]                                                                                      
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userServices.Register(request);

            if (!result)
            {
                return BadRequest("Register is unsuccesful");
            }
            return Ok();

        }
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery]GetUserPaginfRequest request)
        {
           var product = await _userServices.GetUserPaging(request);    
            return Ok(product);
        }

    }
}
