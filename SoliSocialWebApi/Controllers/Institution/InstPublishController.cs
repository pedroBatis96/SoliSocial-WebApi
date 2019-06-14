using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SoliSocialWebApi.Models;
using SoliSocialWebApi.Services.Abstraction;
using SoliSocialWebApi.ViewModels.InstitutionManagement;
using SoliSocialWebApi.ViewModels.News;

namespace SoliSocialWebApi.Controllers.Institution
{
    [Route("api/institution/[controller]")]
    [ApiController]
    public class InstPublishController : ControllerBase
    {
        readonly IAuthService service;
        readonly SoliSocialDbContext context;

        public InstPublishController(IAuthService service, SoliSocialDbContext context)
        {
            this.service = service;
            this.context = context;
        }


        [HttpPost("publishNews")]
        public ActionResult<string> PostTry([FromBody]PublishNews model)
        {
            try
            {
                string userId = context.TdUsers.FirstOrDefault(t => t.Id.ToString() == User.Claims.First().Value).Id;
                TdNoticias newNoticia = new TdNoticias
                {
                    Banner = model.Banner,
                    Corpo = model.Corpo,
                    DataCriacao = DateTime.Now,
                    InstId = model.InstId,
                    Resumo = model.Descricao,
                    Nome = model.Titulo,
                    CriadoPor = userId

                };
                context.TdNoticias.Add(newNoticia);
                context.SaveChanges();
                foreach (var imagem in model.ImageList)
                {
                    context.TaNoticiaImagens.Add(
                        new TaNoticiaImagens
                        {
                            Descricao = imagem.Descricao,
                            Image = imagem.Image,
                            NoticiaId = newNoticia.Id,
                        });
                }
                context.SaveChanges();
                return JsonConvert.SerializeObject("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }
        }

        [HttpPost("getNews")]
        public ActionResult<string> Post([FromBody]PublishNewsGet model)
        {
            try
            {
                string userId = context.TdUsers.FirstOrDefault(t => t.Id.ToString() == User.Claims.First().Value).Id;
                var result = context.TdNoticias.Where(t => model.NewsId == t.Id).Select(
                    t=>new { inst = new { t.Inst.Logo,t.Inst.Acronimo, t.Inst.Nome, t.Inst.Id },
                        t.Nome,t.Resumo,t.Corpo,t.TaNoticiaImagens,t.Banner,t.DataCriacao,t.DataAlteracao
                    }
                ).FirstOrDefault();
                return JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }
        }

        [HttpPost("getNewsByInst")]
        public ActionResult<string> GetNewsByInst([FromBody]InstitutionMain model)
        {
            try
            {
                var result = context.TdNoticias.Where(t => t.InstId == model.InstId)
                    .Select(t => new {t.Inst.Logo,t.DataCriacao, t.Id, t.Nome, t.Banner, t.TaNoticiaImagens.FirstOrDefault().Image })
                    .OrderByDescending(t=>t.DataCriacao);
                return JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }
        }

        [HttpPost("delNews")]
        public ActionResult<string> DelNews([FromBody]PublishNewsGet model)
        {
            try
            {
                var result = context.TdNoticias.Include(i => i.TaNoticiaImagens).Where(t => t.Id == model.NewsId).FirstOrDefault();
                foreach (var image in result.TaNoticiaImagens)
                {
                    context.TaNoticiaImagens.Remove(image);
                }
                context.SaveChanges();
                context.TdNoticias.Remove(result);
                context.SaveChanges();
                return JsonConvert.SerializeObject("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                return (BadRequest(new { err = "Ocorreu um erro, por favor tente mais tarde" }));
            }
        }

    }
}