using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoliSocialWebApi.Models;
using SoliSocialWebApi.Services;
using SoliSocialWebApi.Services.Abstraction;
using SoliSocialWebApi.ViewModels;
using SoliSocialWebApi.ViewModels.Consts;
using SoliSocialWebApi.ViewModels.InstitutionManagement;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace SoliSocialWebApi.Controllers
{
    [Route("api/institution/[controller]")]
    [Authorize]
    [ApiController]
    public class InstManagDataController : ControllerBase
    {
        readonly IAuthService service;
        readonly SoliSocialDbContext context;

        public InstManagDataController(IAuthService service, SoliSocialDbContext context)
        {
            this.service = service;
            this.context = context;
        }

        [HttpPost("dados")]
        public ActionResult<string> Post([FromBody]InstitutionGet model)
        {
            TdUsers user = context.TdUsers.FirstOrDefault(t => t.Id.ToString() == User.Claims.First().Value);
            if (user == null)
            {
                return BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" });
            }
            var instituicao = context.TdInstituicao.Select(t => new
            {
                t.Id,
                t.Logo,
                t.Iban,
                t.Nif,
                t.Morada,
                t.Pagina,
                t.Phonenumber,
                t.Email,
                t.Descricao,
                t.Acronimo,
                t.CodigoPostal,
                t.DataCriacao,
                t.Nome
            }
            ).FirstOrDefault(t => t.Id == model.Id);
            var departamentos = context.TdDepartamentosInstituicao.Where(t => t.InstituicaoId == model.Id).Select(t => new { t.Id, t.Descricao }).ToList();

            try
            {
                return JsonConvert.SerializeObject(new { instituicao, departamentos });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }
        }


        [HttpPost("register")]
        public ActionResult<bool> Post([FromBody]InstitutionRegister model)
        {
            if (!ModelState.IsValid)
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }

            if (context.TdInstituicao.FirstOrDefault(t => t.Nome == model.Name || t.Acronimo == model.Acronimo) != null)
            {
                return (BadRequest(new { err = "Já existe uma instituição com o mesmo nome ou acrónimo" }));
            }

            try
            {
                TdUsers user = context.TdUsers.FirstOrDefault(t => t.Id.ToString() == User.Claims.First().Value);
                TdInstituicao instituicao = new TdInstituicao
                {
                    Id = Guid.NewGuid().ToString(),
                    Acronimo = model.Acronimo,
                    CriadoPor = user.Id,
                    DataCriacao = DateTime.Now,
                    Descricao = model.Descricao,
                    Email = model.Email,
                    Iban = model.Iban,
                    Logo = model.Logo,
                    Morada = model.Morada,
                    Nif = model.Nif,
                    Pagina = model.Pagina,
                    Nome = model.Name,
                    Phonenumber = model.Telemovel,
                    CodigoPostal = model.CodigoPostal

                };
                context.TdInstituicao.Add(instituicao);
                context.SaveChanges();

                string newInstId = instituicao.Id;
                string userRoleCode = Codes.UserRolesCodes.InstAdmin;
                string userDepartmentCodeAdmin = Defaults.UserInstRoles.Admin;
                string userDepartmentCodeVolun = Defaults.UserInstRoles.Voluntário;
                string userRoleId = context.TdUserRoles.FirstOrDefault(t => t.Name == userRoleCode).Id;

                if (!context.TaUserRoles.Any(t => t.UserId == user.Id && t.RoleId == userRoleId))
                {
                    TaUserRoles userRole = new TaUserRoles
                    {
                        UserId = user.Id,
                        RoleId = userRoleId
                    };
                    context.TaUserRoles.Add(userRole);
                }

                var administracao = new TdDepartamentosInstituicao
                {
                    IdPai = null,
                    Descricao = userDepartmentCodeAdmin,
                    InstituicaoId = newInstId,
                };
                var voluntarios = new TdDepartamentosInstituicao
                {
                    IdPai = null,
                    Descricao = userDepartmentCodeVolun,
                    InstituicaoId = newInstId,
                };
                context.TdDepartamentosInstituicao.Add(administracao);
                context.TdDepartamentosInstituicao.Add(voluntarios);
                context.SaveChanges();

                TaStaffInstituicao staffInstituicao = new TaStaffInstituicao
                {
                    DepartamentoId = administracao.Id,
                    InstituicaoId = instituicao.Id,
                    UserId = user.Id,
                };

                context.TaStaffInstituicao.Add(staffInstituicao);
                context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }
        }

        [HttpPost("updatedesc")]
        public ActionResult<bool> Post([FromBody]InstitutionDescUpdate model)
        {
            if (!ModelState.IsValid)
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }
            var user = context.TdUsers.FirstOrDefault(t => t.Id.ToString() == User.Claims.First().Value);

            var passwordValid = service.VerifyPassword(model.Password, user.PasswordHash);
            if (!passwordValid)
            {
                return (BadRequest(new { err = "Password errada" }));
            }
            var institution = context.TdInstituicao.FirstOrDefault(t => t.Id == model.Id);

            if (!string.IsNullOrWhiteSpace(model.Nome))
            {
                if (context.TdInstituicao.FirstOrDefault(t => t.Nome == model.Nome) != null)
                {
                    return (BadRequest(new { err = "Já existe uma instituição com esse nome" }));
                }
                institution.Nome = model.Nome;
            }

            if (!string.IsNullOrWhiteSpace(model.Acron))
            {
                if (context.TdInstituicao.FirstOrDefault(t => t.Acronimo == model.Acron) != null)
                {
                    return (BadRequest(new { err = "Já existe uma instituição com esse nome" }));
                }
                institution.Acronimo = model.Acron;
            }

            if (!string.IsNullOrWhiteSpace(model.Desc))
            {
                institution.Descricao = model.Desc;
            }

            institution.DataAlteracao = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        [HttpPost("updatefisc")]
        public ActionResult<bool> Post([FromBody]InstitutionFiscUpdate model)
        {
            if (!ModelState.IsValid)
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }

            var user = context.TdUsers.FirstOrDefault(t => t.Id.ToString() == User.Claims.First().Value);

            var passwordValid = service.VerifyPassword(model.Password, user.PasswordHash);
            if (!passwordValid)
            {
                return (BadRequest(new { err = "Password errada" }));
            }

            var institution = context.TdInstituicao.FirstOrDefault(t => t.Id == model.Id);
            if (!string.IsNullOrWhiteSpace(model.Nif))
            {
                institution.Nif = model.Nif;
            }

            if (!string.IsNullOrWhiteSpace(model.Iban))
            {
                if (context.TdInstituicao.FirstOrDefault(t => t.Iban == model.Iban) != null)
                {
                    return (BadRequest(new { err = "Já existe uma instituição com esse IBAN" }));
                }
                institution.Iban = model.Iban;
            }

            institution.DataAlteracao = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        [HttpPost("updatecon")]
        public ActionResult<bool> Post([FromBody]InstitutionConUpdate model)
        {
            if (!ModelState.IsValid)
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }

            var user = context.TdUsers.FirstOrDefault(t => t.Id.ToString() == User.Claims.First().Value);

            var passwordValid = service.VerifyPassword(model.Password, user.PasswordHash);
            if (!passwordValid)
            {
                return (BadRequest(new { err = "Password errada" }));
            }

            var institution = context.TdInstituicao.FirstOrDefault(t => t.Id == model.Id);
            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                institution.Email = model.Email;
            }

            if (!string.IsNullOrWhiteSpace(model.Numero))
            {
                institution.Phonenumber = model.Numero;
            }

            if (!string.IsNullOrWhiteSpace(model.Pagina))
            {
                institution.Pagina = model.Pagina;
            }

            if (!string.IsNullOrWhiteSpace(model.CodPostal))
            {
                institution.CodigoPostal = model.CodPostal;
            }

            if (!string.IsNullOrWhiteSpace(model.Morada))
            {
                institution.Morada = model.Morada;
            }

            institution.DataAlteracao = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        [HttpPost("updatelog")]
        public ActionResult<bool> Post([FromBody]InstitutionLogUpdate model)
        {
            if (!ModelState.IsValid)
            {
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }

            var user = context.TdUsers.FirstOrDefault(t => t.Id.ToString() == User.Claims.First().Value);

            var passwordValid = service.VerifyPassword(model.Password, user.PasswordHash);
            if (!passwordValid)
            {
                return (BadRequest(new { err = "Password errada" }));
            }

            var institution = context.TdInstituicao.FirstOrDefault(t => t.Id == model.Id);
            if (!string.IsNullOrWhiteSpace(model.Logo))
            {
                institution.Logo = model.Logo;
            }

            institution.DataAlteracao = DateTime.Now;
            context.SaveChanges();
            return true;
        }
    }
}