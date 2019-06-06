using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SoliSocialWebApi.Models;
using SoliSocialWebApi.Services.Abstraction;
using SoliSocialWebApi.ViewModels.UserManagement;
using System;
using System.Linq;

namespace SoliSocialWebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthService service;
        readonly SoliSocialDbContext context;

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
            var user = context.TdUsers.Include(t => t.TaStaffInstituicao)
                                       .Select(t => new
                                       {
                                           t.Id,
                                           t.DateOfBirth,
                                           t.Email,
                                           t.EmailConfirmed,
                                           t.Genero,
                                           t.Imagem,
                                           t.Name,
                                           t.Bio,
                                           t.Phonenumber,
                                           t.PhonenumberConfirmed,
                                           t.Username,
                                           InstituicaoList = t.TaStaffInstituicao.Where(y => y.UserId == t.Id).Select(y => new { y.Instituicao.Acronimo, y.Instituicao.Id, y.Instituicao.Logo })
                                       })
                                       .FirstOrDefault(t => t.Id == User.Claims.First().Value);
            return JsonConvert.SerializeObject(user);
        }

        [HttpGet("userFavBlock")]
        public ActionResult<string> GetFavBlock()
        {
            string userId = User.Claims.First().Value;
            var favorites = context.TdInstituicao.Where(y => y.TaUserInstituicaoFav.Any(fav => fav.UserId == userId)).Select(y=>new { y.Id, y.Logo, y.Nome });
            var block = context.TdInstituicao.Where(y => y.TaUserInstituicaoBlock.Any(fav => fav.UserId == userId)).Select(y => new { y.Id, y.Logo, y.Nome });
            return JsonConvert.SerializeObject(new { favorites,block });
        }

        /// <summary>
        /// Gets info of user by Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("userId")]
        public ActionResult<string> GetUserById([FromBody]GetUserInfo model)
        {
            var user = context.TdUsers.Select(t => new { t.Imagem, t.Bio, t.Username, t.Id, t.Genero, t.Age, t.Email, t.Phonenumber }).FirstOrDefault(t => t.Id == model.UserId);
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
            var user = context.TdUsers.FirstOrDefault(t => t.Id.ToString() == User.Claims.First().Value);

            if (!service.VerifyPassword(model.Password, user.PasswordHash))
            {
                return (BadRequest(new { err = "Password errada" }));
            }

            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                if (context.TdUsers.FirstOrDefault(t => t.Email == model.Email) != null)
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
            var user = context.TdUsers.FirstOrDefault(t => t.Id.ToString() == User.Claims.First().Value);

            if (!service.VerifyPassword(model.PasswordOld, user.PasswordHash))
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
            var user = context.TdUsers.FirstOrDefault(t => t.Id.ToString() == User.Claims.First().Value);

            if (!service.VerifyPassword(model.Password, user.PasswordHash))
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
            var pertence = context.TaStaffInstituicao.Where(t => t.UserId == model.UserId).Select(t => new { instId = t.InstituicaoId, t.Departamento.Id, t.Departamento.Descricao, t.Instituicao.Logo, t.Instituicao.Acronimo });
            pertence = pertence.OrderBy(t => t.Acronimo);

            var favoritos = context.TaUserInstituicaoFav.Where(t => t.UserId == model.UserId).Select(t => new { t.Instituicao.Id, t.Instituicao.Logo, t.Instituicao.Acronimo });
            favoritos = favoritos.OrderBy(t => t.Acronimo);

            return JsonConvert.SerializeObject(new { pertence, favoritos });
        }

        //private UserInfoSent mapToUser(TdUsers user)
        //{
        //    var result = new UserInfoSent
        //    {
        //        Id = user.Id,
        //        DateOfBirth = user.DateOfBirth,
        //        Email = user.Email,
        //        EmailConfirmed = user.EmailConfirmed,
        //        Genero = user.Genero,
        //        Imagem = user.Imagem,
        //        Name = user.Name,
        //        Bio = user.Bio,
        //        Phonenumber = user.Phonenumber,
        //        PhonenumberConfirmed = user.PhonenumberConfirmed,
        //        Username = user.Username,
        //    };
        //    result.InstituicaoList = new List<UserInfoSent.tdInsituicaoInfo>();
        //    foreach (var instituicao in user.TaStaffInstituicao)
        //    {
        //        var inst = context.TdInstituicao.Where(t => t.Id == instituicao.InstituicaoId).FirstOrDefault();
        //        result.InstituicaoList.Add(new UserInfoSent.tdInsituicaoInfo
        //        {
        //            Acronimo = inst.Acronimo,
        //            Id = inst.Id,
        //            Logo = inst.Logo
        //        });
        //    }

        //    return result;
        //}
    }





}