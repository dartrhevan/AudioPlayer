using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace AuthService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : ControllerBase
    {
        private readonly ILogger<MainController> _logger;

        public MainController(ILogger<MainController> logger)
        {
            _logger = logger;
        }

        private async Task Authenticate(string userName, bool isExtended)
        {
            var id = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, isExtended ? AuthService.User.ExtendedUserRole : AuthService.User.BasicUserRole )
                },
                "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
