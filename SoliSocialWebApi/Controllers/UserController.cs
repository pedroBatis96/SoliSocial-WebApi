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
                                           Favoritos = t.TaUserInstituicaoFav.Select(fav => new { fav.Instituicao.Logo, fav.Instituicao.Acronimo, fav.InstituicaoId }),
                                           Bloqueados = t.TaUserInstituicaoBlock.Select(block => new { block.InstituicaoId }),
                                           InstituicaoList = t.TaStaffInstituicao.Where(y => y.UserId == t.Id).Select(y => new { y.Instituicao.Acronimo, y.Instituicao.Id, y.Instituicao.Logo })
                                       })
                                       .FirstOrDefault(t => t.Id == User.Claims.First().Value);
            return JsonConvert.SerializeObject(user);
        }

        [HttpGet("userFavBlock")]
        public ActionResult<string> GetFavBlock()
        {
            string userId = User.Claims.First().Value;
            var favorites = context.TdInstituicao.Where(y => y.TaUserInstituicaoFav.Any(fav => fav.UserId == userId)).Select(y => new { y.Id, y.Logo, y.Nome });
            var block = context.TdInstituicao.Where(y => y.TaUserInstituicaoBlock.Any(fav => fav.UserId == userId)).Select(y => new { y.Id, y.Logo, y.Nome });
            return JsonConvert.SerializeObject(new { favorites, block });
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
        /// Gets info of user by Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("getUserFeed")]
        public ActionResult<string> GetUserFeed([FromBody]GetUserFeed model)
        {
            var user = context.TdUsers.FirstOrDefault(t => t.Id == User.Claims.First().Value);

            if (model.InstId == "0")
            {
                try
                {
                    var result = context.TdNoticias.Where(t => t.Inst.TaUserInstituicaoBlock.All(bl => bl.UserId != user.Id))
                        .Select(t=>new {InstId=t.Inst.Id,t.Id,t.DataCriacao,t.Nome,t.Resumo,t.Banner,t.Inst.Acronimo,t.Inst.Logo}).OrderByDescending(t=>t.DataCriacao);

                    return JsonConvert.SerializeObject(result);
                }
                catch(Exception ex)
                {
                    return (BadRequest(new { err = "" }));
                }
            }
            else
            {
                var result = context.TdNoticias.Where(t => t.InstId == model.InstId)
                    .Select(t => new { InstId = t.Inst.Id, t.Id, t.DataCriacao, t.Nome, t.Resumo, t.Banner, t.Inst.Acronimo, t.Inst.Logo })
                    .OrderByDescending(t => t.DataCriacao);
                return JsonConvert.SerializeObject(result);
            }
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

    }





}