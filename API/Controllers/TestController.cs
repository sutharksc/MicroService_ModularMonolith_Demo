using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedModule.Abstractions;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TestController(IUserContext userContext) : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            Console.WriteLine(userContext.IsAuthenticated);
            return Ok("Running");
        }
    }
}
