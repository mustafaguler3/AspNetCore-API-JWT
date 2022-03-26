using Microsoft.AspNetCore.Mvc;
using Udemy.Shared.Dtos;

namespace Udemy.AuthServerAPI.Controllers
{
    public class CustomBaseController : ControllerBase
    {
        public IActionResult ActionResultInstance<T>(Response<T> response)where T :class
        {
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
    }
}
