using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SoliSocialWebApi.Models;
using SoliSocialWebApi.Services.Abstraction;
using SoliSocialWebApi.ViewModels.InstitutionManagement;
using SoliSocialWebApi.ViewModels.News;
using System;
using System.Linq;

namespace SoliSocialWebApi.Controllers.Institution
{
    [Route("api/institution/[controller]")]
    [ApiController]
    [Authorize]
    public class InstitutionMainController : ControllerBase
    {
        readonly IAuthService service;
        readonly SoliSocialDbContext context;

        public InstitutionMainController(IAuthService service, SoliSocialDbContext context)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("instList")]
        public ActionResult<string> Get()
        {
            string userId = context.TdUsers.FirstOrDefault(t => t.Id.ToString() == User.Claims.First().Value).Id;
            var institutionList = context.TdInstituicao.Where(t => t.TaUserInstituicaoBlock.All(y => y.UserId != userId)).OrderBy(t => t.DataCriacao).Select(t => new { t.Id, t.Nome, t.Logo });
            return JsonConvert.SerializeObject(institutionList);
        }

        [HttpPost("getById")]
        public ActionResult<string> Post([FromBody]InstitutionMain model)
        {
            try
            {

                string userId = User.Claims.First().Value;
                bool favorited = false;

                var instituicao = context.TdInstituicao
                                  .Include(si => si.TaStaffInstituicao)
                                  .Include(n => n.TdNoticias)
                                  .Include(e => e.TdEvento)
                                  .Include(f => f.TaUserInstituicaoFav)
                                  .Select(t=> new {t.Id, t.Nome,t.Logo,t.TaStaffInstituicao,t.TdNoticias,t.TdEvento,t.TaUserInstituicaoFav,t.Descricao,t.Email,t.Phonenumber,t.CodigoPostal,t.Morada,t.Pagina})
                                  .FirstOrDefault(t => t.Id == model.InstId.ToString());

                if (instituicao.TaUserInstituicaoFav.FirstOrDefault(t => t.UserId == userId && t.InstituicaoId == model.InstId) != null)
                {
                    favorited = true;
                }

                foreach (var staff in instituicao.TaStaffInstituicao)
                {
                    staff.Instituicao = null;
                }

                foreach (var fav in instituicao.TaUserInstituicaoFav)
                {
                    fav.Instituicao = null;
                }

                return JsonConvert.SerializeObject(new { instituicao, favorited });
            }
            catch (Exception ex)
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }
        }


        [HttpPost("handleFavorite")]
        public ActionResult<bool> HandleFavorite([FromBody]InstitutionMain model)
        {
            try
            {
                string userId = User.Claims.First().Value;

                var favorited = context.TaUserInstituicaoFav.FirstOrDefault(t => t.UserId.ToString() == userId && t.InstituicaoId == model.InstId.ToString());

                if (favorited != null)
                {
                    context.TaUserInstituicaoFav.Remove(favorited);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    TaUserInstituicaoFav favoritedNew = new TaUserInstituicaoFav
                    {
                        InstituicaoId = model.InstId.ToString(),
                        UserId = userId
                    };
                    context.TaUserInstituicaoFav.Add(favoritedNew);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }
        }

        [HttpPost("removeInstFavorite")]
        public ActionResult<bool> HandleRemoveFavorite([FromBody]InstitutionMain model)
        {
            try
            {
                string userId = User.Claims.First().Value;

                var favorited = context.TaUserInstituicaoFav.FirstOrDefault(t => t.UserId.ToString() == userId && t.InstituicaoId == model.InstId.ToString());

                if (favorited != null)
                {
                    context.TaUserInstituicaoFav.Remove(favorited);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }
        }

        [HttpPost("removeInstBan")]
        public ActionResult<bool> HandleRemoveBan([FromBody]InstitutionMain model)
        {
            try
            {
                string userId = User.Claims.First().Value;

                var blocked = context.TaUserInstituicaoBlock.FirstOrDefault(t => t.UserId.ToString() == userId && t.InstituicaoId == model.InstId.ToString());

                if (blocked != null)
                {
                    context.TaUserInstituicaoBlock.Remove(blocked);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }
        }

        [HttpPost("getAllMembers")]
        public ActionResult<string> getAllMembers([FromBody]InstitutionMain model)
        {
            try
            {
                string userId = User.Claims.First().Value;

                var membros = context.TaStaffInstituicao
                                  .Include(d => d.Departamento)
                                  .Where(ta => ta.InstituicaoId == model.InstId)
                                  .Select(t => new { t.User.Id, t.User.Username, t.User.Imagem, t.Departamento.Descricao })
                                  .OrderBy(t => t.Username);

                var departamentos = context.TdDepartamentosInstituicao.Where(t => t.InstituicaoId == model.InstId).Select(t => new { t.Descricao, t.Id });

                return JsonConvert.SerializeObject(new { membros, departamentos });
            }
            catch (Exception ex)
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }
        }
    }
}