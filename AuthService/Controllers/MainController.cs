using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace AuthService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : ControllerBase
    {
        private readonly ILogger<MainController> _logger;

        public string Index() => "Auth service";

        public MainController(ILogger<MainController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<User> Login([FromForm]string login, [FromForm]string password)
        {/*
            if (users.Any(u => u.Email == user.Email && u.Password == user.Password))
            {
                Authenticate(user.Email);
                return User.Identity.Name;
            }
            return "Incorrect data";*/
            _logger.Log(LogLevel.Debug,  "Login");

            using (var db = new AuthDbContext())
            {
                var user = db.Users.Find(login);
                if (user != null && ArrayEquals(user.PasswordHash, AuthService.User.Encrypt(password)))
                {

                    await Authenticate(user.Login, user.IsExtended);
                    return user;
                }
            }
            return null;
        }

        private static bool ArrayEquals<T>(T[] arr1, T[] arr2)
        {
            if (arr1.Length != arr2.Length) return false;
            for (var i = 0; i < arr1.Length; i++)
                if (!arr1[i].Equals(arr2[i])) return false;
            return true;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<string> Register([FromForm]string login, [FromForm]string password, [FromForm]string licenseKey = null)
        {
            _logger.Log(LogLevel.Debug, "Register");
            using (var db = new AuthDbContext())
            {
                var u = db.Users.Find(login);
                if (u != null)
                    return "Already exists";
                var user = new User(login, AuthService.User.Encrypt(password),
                    licenseKey == AuthService.User.LicenseKey);
                db.Users.Add(user);
                db.SaveChangesAsync();
                await Authenticate(user.Login, user.IsExtended);
                return "OK";
            }
        }

        [Route("Test")]
        public string Test() => "szx"; 

        [HttpPut]
        public string EditSettings(string login, User user)
        {
            using (var db = new AuthDbContext())
            {
                var u = db.Users.Find(login);
                if (u == null) return "Not found";
                u.Login = user.Login ?? u.Login;
                u.IsSimple = user.IsSimple;
                u.Volume = user.Volume;
                u.MainDirectory = user.MainDirectory ?? u.MainDirectory;
                db.Entry(u).State = EntityState.Modified;
                db.SaveChangesAsync();
                return "OK";
            }
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
