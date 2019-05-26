using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SoliSocialWebApi.Models;
using SoliSocialWebApi.Services;
using SoliSocialWebApi.Services.Abstraction;
using SoliSocialWebApi.ViewModels;

namespace SoliSocialWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService service;
        private SoliSocialDbContext context;
        private static Random random = new Random();

        public AuthController(IAuthService authService, SoliSocialDbContext context)
        {
            this.context = context;
            service = authService;
        }


        [HttpPost("Login")]
        [Route("Login")]
        public ActionResult<AuthData> Post([FromBody] LoginModel login)
        {
            ApiAuthHeader header = HMACAuthorization.GetApiAuthHeader(HttpContext);
            if (header == null)
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }

            if (!ModelState.IsValid)
            {
                return (BadRequest(new { err = ModelState }));
            }

            HMACAuthorization apiAuth = new HMACAuthorization(context);
            if (!apiAuth.Chalenge(header, "POST", HMACAuthorization.ReadBody(HttpContext)))
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }

            var user = context.TdUsers.FirstOrDefault(t => t.Email == login.Email);

            if (user == null)
            {
                return (BadRequest(new { err = "Utilizador não existe, por favor registe-se" }));
            }

            var passwordValid = service.VerifyPassword(login.Password, user.PasswordHash);

            if (!passwordValid)
            {
                return (BadRequest(new { err = "Password errada" }));
            }

            var authData = service.GetAuthData(user.Id.ToString());
            authData.Email = user.Email;
            authData.Username = user.Username;

            return authData;
        }

        [HttpPost("Registo")]
        [Route("Registo")]
        public ActionResult<AuthData> Post([FromBody] RegistoModel registo)
        {
            ApiAuthHeader header = HMACAuthorization.GetApiAuthHeader(HttpContext);
            if (header == null)
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }

            if (!ModelState.IsValid)
            {
                return (BadRequest(new { err = ModelState }));
            }

            HMACAuthorization apiAuth = new HMACAuthorization(context);
            if (!apiAuth.Chalenge(header, "POST", HMACAuthorization.ReadBody(HttpContext)))
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }

            if (context.TdUsers.FirstOrDefault(t => t.Email == registo.Email) != null)
            {
                return (BadRequest(new { err = "Conta com esse endereço de email já existe" }));
            }

            var Newuser = new TdUsers
            {
                Name = registo.Name,
                NormalizedName = registo.Name.ToUpper(),
                Username = CalcUsername(registo.Name),
                PasswordHash = service.HashPassword(registo.Password),
                DataCriacao = DateTime.Now,
                DateOfBirth = registo.BirthDate,
                Age = CalcAge(registo.BirthDate),
                Email = registo.Email,
                EmailConfirmed = false,
                Genero = registo.Gender,
                ConcurrencyStamp = RandomString(36)
            };

            context.TdUsers.Add(Newuser);
            context.SaveChanges();

            var authData = service.GetAuthData(Newuser.Id.ToString());
            authData.Email = Newuser.Email;
            authData.Username = Newuser.Username;

            return authData;
        }

        
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        private int CalcAge(DateTime BirthDate)
        {
            int age = DateTime.Now.Year - BirthDate.Year;

            if ((BirthDate.Month > DateTime.Now.Month) || (BirthDate.Month == DateTime.Now.Month && BirthDate.Day > DateTime.Now.Day))
                age--;

            return age;
        }

        private string CalcUsername(string name)
        {
            var usernameAux = name.Split(" ");
            string username;
            if (usernameAux.Length > 1)
            {
                username = usernameAux.FirstOrDefault() + " " + usernameAux.Last();
            }
            else
            {
                username = usernameAux.FirstOrDefault();
            }

            return username;
        }

    }
}
