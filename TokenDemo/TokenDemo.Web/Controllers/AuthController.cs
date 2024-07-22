using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TokenDemo.Web.Models;
using TokenDemo.Web.Services;

namespace TokenDemo.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        public AuthController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody]LoginModel loginModel)
        {
            var result = await _identityService.LoginAsync(loginModel);
            return Ok(result);
        }
    }
}
