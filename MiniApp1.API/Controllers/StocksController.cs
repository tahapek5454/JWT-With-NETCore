using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MiniApp1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {

        [HttpGet]
        [Authorize(Roles ="customer")]
        public IActionResult GetStock()
        {
            // you can access username, id , email etc. from jwt claims with Identity rules
            var userName = User.Identity.Name;
            var userId = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value;
            // we set id with nameIdentifier but .Identity.Name is usual so we can access easliy


            // for example fake db getting data with byUserName or byId
            // this just for example
            var controllerName = "MiniApp1";
            return Ok(new { controllerName,userId, userName});
        }
    }
}
