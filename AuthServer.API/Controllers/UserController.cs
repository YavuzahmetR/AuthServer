using AuthServer.Core.Dto_s;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Exceptions;

namespace AuthServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : CustomBaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            throw new CustomException("Database Error Occured");
            return ActionResultInstance(await _userService.CreateUserAsync(createUserDto));
        }



        [Authorize] //Token Needed
        [HttpGet]
        public async Task<IActionResult> GetUser()//No Parameter Needed Because GetClaims Method in TokenService includes "Name" Claim in token payload. 
        {
            return ActionResultInstance(await _userService.GetUserByNameAsync(HttpContext.User.Identity.Name)); //Extracts the "name" claim in the token payload.
            
        }
    }
}
