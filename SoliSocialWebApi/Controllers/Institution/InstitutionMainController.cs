using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SoliSocialWebApi.Models;
using SoliSocialWebApi.Services;
using SoliSocialWebApi.Services.Abstraction;
using SoliSocialWebApi.ViewModels;
using SoliSocialWebApi.ViewModels.InstitutionManagement;

namespace SoliSocialWebApi.Controllers.Institution
{
    [Route("api/institution/[controller]")]
    [ApiController]
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

            TdUsers user = context.TdUsers.Include(t => t.TdInstituicao).Include(t => t.TaStaffInstituicao).FirstOrDefault(t => t.Id.ToString() == User.Claims.First().Value);
            var institutionList = context.TdInstituicao.OrderBy(t=>t.DataCriacao).Select(t => new { t.Id, t.Nome, t.Logo });
            return JsonConvert.SerializeObject(institutionList);
        }

        [HttpPost("getById")]
        public ActionResult<string> Post([FromBody]InstitutionMain model)
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

            try
            {

                string userId = User.Claims.First().Value;
                bool favorited = false;

                var instituicao = context.TdInstituicao
                                  .Include(si => si.TaStaffInstituicao)
                                  .Include(n => n.TdNoticias)
                                  .Include(e => e.TdEvento)
                                  .Include(f => f.TaUserInstituicaoFav)
                                  .FirstOrDefault(t => t.Id == model.InstId);

                if (instituicao.TaUserInstituicaoFav.FirstOrDefault(t => t.UserId.ToString() == userId) != null)
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

            try
            {
                string userId = User.Claims.First().Value;

                var favorited = context.TaUserInstituicaoFav.FirstOrDefault(t => t.UserId.ToString() == userId && t.InstituicaoId == model.InstId);

                if(favorited != null)
                {
                    context.TaUserInstituicaoFav.Remove(favorited);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    TaUserInstituicaoFav favoritedNew = new TaUserInstituicaoFav
                    {
                        InstituicaoId = model.InstId,
                        UserId = Guid.Parse(userId)
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

        [HttpPost("getAllMembers")]
        public ActionResult<string> getAllMembers([FromBody]InstitutionMain model)
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

            try
            {
                string userId = User.Claims.First().Value;

                var membros = context.TaStaffInstituicao
                                  .Include(d => d.Departamento)
                                  .Where(ta => ta.InstituicaoId == model.InstId)
                                  .Select(t => new { t.User.Id, t.User.Username, t.User.Imagem, t.Departamento.Descricao })
                                  .OrderBy(t=>t.Username);

                var departamentos = context.TdDepartamentosInstituicao.Where(t => t.InstituicaoId == model.InstId).Select(t=>new { t.Descricao, t.Id });

                return JsonConvert.SerializeObject(new { membros,departamentos });
            }
            catch (Exception ex)
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }
        }
    }
}