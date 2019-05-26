using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SoliSocialWebApi.Models;
using SoliSocialWebApi.Services;
using SoliSocialWebApi.Services.Abstraction;
using SoliSocialWebApi.ViewModels;
using SoliSocialWebApi.ViewModels.Consts;
using SoliSocialWebApi.ViewModels.InstitutionManagement.Mem;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SoliSocialWebApi.Controllers.Institution
{
    [Route("api/institution/[controller]")]
    [Authorize]
    [ApiController]
    public class InstManagMemController : ControllerBase
    {
        readonly IAuthService service;
        readonly SoliSocialDbContext context;

        public InstManagMemController(IAuthService service, SoliSocialDbContext context)
        {
            this.service = service;
            this.context = context;
        }

        [HttpPost("nonmembers")]
        public ActionResult<string> Post([FromBody]InstManagMemNonMemGet model)
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

            TdUsers user = context.TdUsers.FirstOrDefault(t => t.Id.ToString() == User.Claims.First().Value);
            var people = context.TdUsers
                .Where(t => t.TaStaffInstituicao.All(y => y.InstituicaoId != model.Id))
                .Where(t => t.Username.Contains(model.Nome) || t.Email == model.Email)
                .Select(t => new { t.Imagem, t.Id, t.Username }).OrderBy(t => t.Username)
                .ToList();

            var departments = context.TdDepartamentosInstituicao.Where(t => t.InstituicaoId == model.Id).Select(t => new { t.Id, t.Descricao });

            try
            {
                return JsonConvert.SerializeObject(new { people });
            }
            catch (Exception ex)
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }
        }

        [HttpPost("addmember")]
        public ActionResult<bool> Post([FromBody]InstManagaDepartAddMem model)
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
                if (context.TaStaffInstituicao.FirstOrDefault(t => t.DepartamentoId == model.IdDepart
                    && t.UserId == model.IdUser && t.InstituicaoId == model.IdInst) != null)
                {
                    return (BadRequest(new { err = "Utilizador já está atribuido" }));
                }

                context.TaStaffInstituicao.Add(new TaStaffInstituicao
                {
                    DepartamentoId = model.IdDepart,
                    InstituicaoId = model.IdInst,
                    UserId = model.IdUser,
                    DataEntrada = DateTime.Now
                });

                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }
        }

        [HttpPost("getmembersRem")]
        public ActionResult<string> Post([FromBody]InstManagDepartGetCurrent model)
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

            TdUsers user = context.TdUsers.FirstOrDefault(t => t.Id.ToString() == User.Claims.First().Value);


            try
            {
                var people = context.TdUsers
                .Include(st => st.TaStaffInstituicao)
                .Where(t => t.TaStaffInstituicao
                .Any(y => model.DepId != 0 ? y.InstituicaoId == model.Id && y.DepartamentoId == model.DepId && y.UserId != user.Id
                : y.InstituicaoId == model.Id && y.UserId != user.Id) && (t.Username.Contains(model.Nome) || t.Email == model.Email))
                .Select(t => new
                {
                    t.Imagem,
                    t.Id,
                    t.Username,
                    dep = t.TaStaffInstituicao.FirstOrDefault(ta => ta.UserId == t.Id && ta.InstituicaoId == model.Id).Departamento
                })
                .OrderBy(t => t.Username)
                .ToList();
                return JsonConvert.SerializeObject(new { people });
            }
            catch (Exception ex)
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }
        }

        [HttpPost("getmembersViz")]
        public ActionResult<string> PostViz([FromBody]InstManagDepartGetCurrent model)
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

            TdUsers user = context.TdUsers.FirstOrDefault(t => t.Id.ToString() == User.Claims.First().Value);


            try
            {
                var people = context.TdUsers
                .Where(t => t.TaStaffInstituicao
                .Any(y => model.DepId != 0 ? y.InstituicaoId == model.Id && y.DepartamentoId == model.DepId && y.UserId != user.Id
                : y.InstituicaoId == model.Id && y.UserId != user.Id) && (t.Username.Contains(model.Nome) || t.Email == model.Email))
                .Select(t => new
                {
                    t.Imagem,
                    t.Id,
                    t.Username,
                    dep = t.TaStaffInstituicao.FirstOrDefault(y => y.UserId == t.Id && y.InstituicaoId == model.Id).Departamento.Descricao,
                    data = t.TaStaffInstituicao.FirstOrDefault(y => y.UserId == t.Id && y.InstituicaoId == model.Id).DataEntrada
                })
                .OrderBy(t => t.Username)
                .ToList();

                return JsonConvert.SerializeObject(new { people });
            }
            catch (Exception ex)
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }
        }

        [HttpPost("remMember")]
        public ActionResult<bool> Post([FromBody]InstManagaDepartRemMem model)
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

            TdUsers user = context.TdUsers.FirstOrDefault(t => t.Id.ToString() == User.Claims.First().Value);
            var ta = context.TaStaffInstituicao.FirstOrDefault(t => t.UserId == model.IdUser && t.InstituicaoId == model.IdInst && t.DepartamentoId == model.IdDepart);
            context.TaStaffInstituicao.Remove(ta);
            context.SaveChanges();
            try
            {
                return true;
            }

            catch (Exception ex)
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }
        }

        [HttpPost("remDepart")]
        public ActionResult<bool> Post([FromBody]InstManagDepartRemDep model)
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
                TdUsers user = context.TdUsers.FirstOrDefault(t => t.Id.ToString() == User.Claims.First().Value);
                TdDepartamentosInstituicao departamento = context.TdDepartamentosInstituicao.FirstOrDefault(t => t.Id == model.IdDepart);

                if (departamento.Descricao == Defaults.UserInstRoles.Admin || departamento.Descricao == Defaults.UserInstRoles.Voluntário)
                {
                    return (BadRequest(new { err = "Não pode apagar os departamentos de administração ou de voluntários" }));
                }

                long newDepartmentId = context.TdDepartamentosInstituicao.FirstOrDefault(t => t.InstituicaoId == model.idInst && t.Descricao == Defaults.UserInstRoles.Voluntário).Id;
                List<TaStaffInstituicao> taList = context.TaStaffInstituicao.Where(t => t.DepartamentoId == model.IdDepart).ToList();

                foreach (var ta in taList)
                {
                    context.TaStaffInstituicao.Remove(ta);
                }
                context.SaveChanges();

                foreach (var ta in taList)
                {
                    ta.DepartamentoId = newDepartmentId;
                    context.TaStaffInstituicao.Add(ta);
                }
                context.SaveChanges();

                context.TdDepartamentosInstituicao.Remove(departamento);
                context.SaveChanges();
                return true;
            }

            catch (Exception ex)
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }
        }

        [HttpPost("addDepart")]
        public ActionResult<bool> Post([FromBody]InstManagDepartAddDep model)
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

            TdUsers user = context.TdUsers.FirstOrDefault(t => t.Id.ToString() == User.Claims.First().Value);

            if (context.TdDepartamentosInstituicao.FirstOrDefault(t => t.Descricao == model.Descricao && t.InstituicaoId == model.IdInst) != null)
            {
                return (BadRequest(new { err = "Já existe esse departamento" }));
            }
            TdDepartamentosInstituicao departamento = new TdDepartamentosInstituicao
            {
                InstituicaoId = model.IdInst,
                Descricao = model.Descricao,
            };

            context.TdDepartamentosInstituicao.Add(departamento);
            context.SaveChanges();
            try
            {
                return true;
            }

            catch (Exception ex)
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }
        }
    }
}