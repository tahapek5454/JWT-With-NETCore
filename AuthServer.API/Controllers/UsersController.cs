using AuthServer.Core.DTOs;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : CustomBaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            var result = await _userService.CreateUserAsync(createUserDto);

            return ActionResultInstance(result);
        }

        [Authorize] //exactly want token
        [HttpGet]
        public async Task<IActionResult> GetUserByName()
        {
            // httpContext finde username from User claims
            var result = await _userService.GetUserByUserName(HttpContext.User.Identity.Name);
            return ActionResultInstance(result);
        }
    }
}
