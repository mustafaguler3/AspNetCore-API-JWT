using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace MiniApp1.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        public IActionResult GetStock()
        {
            var username = HttpContext.User.Identity.Name;
            var userId = User.Claims.FirstOrDefault(i => i.Type == ClaimTypes.NameIdentifier);

            return Ok($"Username :{username} - UserId:{userId}");
        }
    }
}
