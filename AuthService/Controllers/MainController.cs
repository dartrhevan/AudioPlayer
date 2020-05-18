using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
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
        [HttpPost]
        public async Task<User> Login(string login, string password)
        {/*
            if (users.Any(u => u.Email == user.Email && u.Password == user.Password))
            {
                Authenticate(user.Email);
                return User.Identity.Name;
            }
            return "Incorrect data";*/
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
                if (arr1[i].Equals(arr2[i])) return false;
            return true;
        }

        [HttpPost]
        public string Register(User user)
        {/*
            Console.WriteLine(user.Email);
            if (users.Any(u => u.Email == user.Email))
                return "Already exists";
            else
            {
                users.Add(user);
                Authenticate(user.Email);
                return User.Identity.Name;
            }*/
            return "";
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
