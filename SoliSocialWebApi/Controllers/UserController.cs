using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SoliSocialWebApi.Models;
using SoliSocialWebApi.Services;
using SoliSocialWebApi.Services.Abstraction;
using SoliSocialWebApi.ViewModels;
using SoliSocialWebApi.ViewModels.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SoliSocialWebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {

        IAuthService service;
        SoliSocialDbContext context;

        public UserController(IAuthService authService, SoliSocialDbContext context)
        {
            this.service = authService;
            this.context = context;
        }

        /// <summary>
        /// Gets info of the current User
        /// </summary>
        /// <returns></returns>
        [HttpGet("me")]
        public ActionResult<string> Get()
        {
            ApiAuthHeader header = HMACAuthorization.GetApiAuthHeader(HttpContext);
            if (header == null)
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }

            if (!ModelState.IsValid)
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }

            HMACAuthorization apiAuth = new HMACAuthorization(context);
            if (!apiAuth.Chalenge(header, "GET"))
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }

            var user = context.TdUsers.Include(t => t.TdInstituicao).Include(t => t.TaStaffInstituicao).FirstOrDefault(t => t.Id.ToString() == User.Claims.First().Value);
            var result = mapToUser(user);
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Gets info of user by Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("userId")]
        public ActionResult<string> GetUserById([FromBody]GetUserInfo model)
        {
            ApiAuthHeader header = HMACAuthorization.GetApiAuthHeader(HttpContext);
            if (header == null)
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }

            if (!ModelState.IsValid)
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }

            HMACAuthorization apiAuth = new HMACAuthorization(context);
            if (!apiAuth.Chalenge(header, "POST", HMACAuthorization.ReadBody(HttpContext)))
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }

            TdUsers user = context.TdUsers.FirstOrDefault(t => t.Id == model.UserId);

            return JsonConvert.SerializeObject(user);
        }

        /// <summary>
        /// Updates user personal info
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("updateinfo")]
        public ActionResult<bool> Post([FromBody] UserInfoUpdate model)
        {
            ApiAuthHeader header = HMACAuthorization.GetApiAuthHeader(HttpContext);
            if (header == null)
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }

            if (!ModelState.IsValid)
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }

            HMACAuthorization apiAuth = new HMACAuthorization(context);
            if (!apiAuth.Chalenge(header, "POST", HMACAuthorization.ReadBody(HttpContext)))
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }
            var user = context.TdUsers.FirstOrDefault(t => t.Id.ToString() == User.Claims.First().Value);

            var passwordValid = service.VerifyPassword(model.Password, user.PasswordHash);
            if (!passwordValid)
            {
                return (BadRequest(new { err = "Password errada" }));
            }

            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                if(context.TdUsers.FirstOrDefault(t => t.Email == model.Email) != null)
                {
                    return (BadRequest(new { err = "Já existe um utilizador com esse email" }));
                }
                user.Email = model.Email;
                user.EmailConfirmed = 0;
            }
            if (!string.IsNullOrWhiteSpace(model.Phonenumber))
            {
                user.Phonenumber = model.Phonenumber;
                user.PhonenumberConfirmed = 0;
            }
            if (!string.IsNullOrWhiteSpace(model.Bio))
            {
                user.Bio = model.Bio;
            }
            if (!string.IsNullOrWhiteSpace(model.Genero))
            {
                user.Genero = model.Genero;
            }

            user.DataAlteracao = DateTime.Now;
            context.SaveChanges();
            return true;
        }
        /// <summary>
        /// Updates user password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("updatepass")]
        public ActionResult<bool> Post([FromBody]UserPassUpdate model)
        {
            ApiAuthHeader header = HMACAuthorization.GetApiAuthHeader(HttpContext);
            if (header == null)
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            HMACAuthorization apiAuth = new HMACAuthorization(context);
            if (!apiAuth.Chalenge(header, "POST", HMACAuthorization.ReadBody(HttpContext)))
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }
            var user = context.TdUsers.FirstOrDefault(t => t.Id.ToString() == User.Claims.First().Value);

            var passwordValid = service.VerifyPassword(model.PasswordOld, user.PasswordHash);
            if (!passwordValid)
            {
                return (BadRequest(new { err = "Password errada" }));
            }
            user.PasswordHash = service.HashPassword(model.PasswordNew);
            user.DataAlteracao = DateTime.Now;
            context.SaveChanges();
            return true;
        }
        /// <summary>
        /// Updates user image
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("updateimage")]
        public ActionResult<bool> Post([FromBody]UserImageUpdate model)
        {
            ApiAuthHeader header = HMACAuthorization.GetApiAuthHeader(HttpContext);
            if (header == null)
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }
            if (!ModelState.IsValid)
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }
            HMACAuthorization apiAuth = new HMACAuthorization(context);
            if (!apiAuth.Chalenge(header, "POST", HMACAuthorization.ReadBody(HttpContext)))
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }
            var user = context.TdUsers.FirstOrDefault(t => t.Id.ToString() == User.Claims.First().Value);

            var passwordValid = service.VerifyPassword(model.Password, user.PasswordHash);
            if (!passwordValid)
            {
                return (BadRequest(new { err = "Password errada" }));
            }

            user.Imagem = model.Image;
            user.DataAlteracao = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        [HttpPost("userInstitution")]
        public ActionResult<string> Post([FromBody]GetUserInfo model)
        {
            ApiAuthHeader header = HMACAuthorization.GetApiAuthHeader(HttpContext);
            if (header == null)
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }

            if (!ModelState.IsValid)
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }

            HMACAuthorization apiAuth = new HMACAuthorization(context);
            if (!apiAuth.Chalenge(header, "POST", HMACAuthorization.ReadBody(HttpContext)))
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }


            var pertence = context.TaStaffInstituicao.Where(t => t.UserId == model.UserId).Select(t => new {instId = t.InstituicaoId,t.Departamento.Id, t.Departamento.Descricao,t.Instituicao.Logo,t.Instituicao.Acronimo });
            pertence = pertence.OrderBy(t => t.Acronimo);

            var favoritos = context.TaUserInstituicaoFav.Where(t => t.UserId == model.UserId).Select(t => new { t.Instituicao.Id, t.Instituicao.Logo, t.Instituicao.Acronimo });
            favoritos = favoritos.OrderBy(t => t.Acronimo);

            return JsonConvert.SerializeObject(new { pertence,favoritos});
        }

        private UserInfoSent mapToUser(TdUsers user)
        {
            var result = new UserInfoSent
            {
                Id = user.Id,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                Genero = user.Genero,
                Imagem = user.Imagem,
                Name = user.Name,
                Bio = user.Bio,
                Phonenumber = user.Phonenumber,
                PhonenumberConfirmed = user.PhonenumberConfirmed,
                Username = user.Username,
            };
            result.InstituicaoList = new List<UserInfoSent.tdInsituicaoInfo>();
            foreach (var instituicao in user.TaStaffInstituicao)
            {
                var inst = context.TdInstituicao.Where(t => t.Id == instituicao.InstituicaoId).FirstOrDefault();
                result.InstituicaoList.Add(new UserInfoSent.tdInsituicaoInfo
                {
                    Acronimo = inst.Acronimo,
                    Id = inst.Id,
                    Logo = inst.Logo
                });
            }

            return result;
        }
    }


    


}