using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MiniApp3.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WheatersController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult GetStock()
        {
            // we dont need subscribe for this api
            // just need token just important audiences and Issiue, signig

            // this just for example
            var controllerName = "MiniApp3";
            var wheater = "sunny";
            return Ok(new { controllerName, wheater });
        }
    }
}
