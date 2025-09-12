using Microsoft.AspNetCore.Mvc;

namespace BulgarianMountainTrails.API.Controllers
{
    public class ErrorController : ControllerBase
    {
        [Route("error")]
        public IActionResult HandleError() =>
        Problem(statusCode: 500, title: "Unexpected error occurred.");
    }
}
