using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using SoliSocialWebApi.Models;
using SoliSocialWebApi.ViewModels;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SoliSocialWebApi.Services
{
    public class AuthorizeApp : ActionFilterAttribute
    {
        private SoliSocialDbContext dbContext;
        public AuthorizeApp(SoliSocialDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            Console.WriteLine("context:  " + context);

            ApiAuthHeader apiAuthHeader = GetApiAuthHeader(context.HttpContext);
            if (apiAuthHeader == null)
            {
                context.Result = new BadRequestObjectResult(new { err = "Ocorreu um erro, por favor tente novamente mais tarde" });
                return;
            }

            //Chalenge the API Header
            bool validApp;
            if (context.HttpContext.Request.Method == "POST")
                validApp = Chalenge(apiAuthHeader, context.HttpContext.Request.Method, ReadBody(context.HttpContext));
            else
                validApp = Chalenge(apiAuthHeader, context.HttpContext.Request.Method);
            if (!validApp)
            {
                context.Result = new BadRequestObjectResult(new { err = "Ocorreu um erro, por favor tente novamente mais tarde" });
                return;
            }
        }

        public bool Chalenge(ApiAuthHeader authHeader, string method, string body = "")
        {

            var appData = dbContext.TdApiClient.FirstOrDefault(ac => ac.Id == authHeader.AppId);

            if (appData == null)
                return false;

            string key = appData.Key;


            byte[] content = Encoding.UTF8.GetBytes(body);
            MD5 md5 = MD5.Create();
            byte[] requestContentHash = md5.ComputeHash(content);

            var calcSignature = "";
            if (body == "")
                calcSignature = authHeader.AppId + method + authHeader.Timestamp + authHeader.Nonce;
            else
                calcSignature = authHeader.AppId + method + authHeader.Timestamp + authHeader.Nonce + requestContentHash.BAToString();
            byte[] calcSignatureBa = Encoding.UTF8.GetBytes(calcSignature);
            byte[] secretKeyByteArray = Encoding.UTF8.GetBytes(key);

            using (HMACSHA256 hmac = new HMACSHA256(secretKeyByteArray))
            {
                byte[] signatureBytes = hmac.ComputeHash(calcSignatureBa);
                string signatureHex = signatureBytes.BAToString();

                if (signatureHex == authHeader.Signature)
                    return true;
                return false;
            }
        }

        private string ReadBody(HttpContext httpContext)
        {
            byte[] body = new byte[httpContext.Request.Body.Length];
            int bodyLength = int.Parse(httpContext.Request.Body.Length.ToString());

            httpContext.Request.Body.Seek(0, System.IO.SeekOrigin.Begin);
            httpContext.Request.Body.Read(body, 0, bodyLength);

            return Encoding.UTF8.GetString(body);
        }

        private ApiAuthHeader GetApiAuthHeader(HttpContext httpContext)
        {
            //START Grab API Auth Header
            var req = httpContext.Request;
            foreach(var x in req.Headers)
            {
                Console.WriteLine(x);
            }

            var apiAuthHeaderStr = req.Headers.FirstOrDefault(h => h.Key == "appauthentication").Value.FirstOrDefault();
            Console.WriteLine("apiAuthHeaderStr:  " + apiAuthHeaderStr);

            if (apiAuthHeaderStr == null)
                return null;

            dynamic api_auth = JObject.Parse(apiAuthHeaderStr);

            return new ApiAuthHeader
            {
                Timestamp = api_auth.timestamp,
                AppId = api_auth.appId,
                Signature = api_auth.signature,
                Nonce = api_auth.nonce
            };
            //END Grab API Auth Header
        }
    }

    public static class ByteArrayExtensions
    {
        public static string BAToString(this byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", "").ToLower();
        }
    }
}
