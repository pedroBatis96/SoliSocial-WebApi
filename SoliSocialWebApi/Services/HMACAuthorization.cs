using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using SoliSocialWebApi.Models;
using SoliSocialWebApi.ViewModels;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SoliSocialWebApi.Services
{
    public class HMACAuthorization
    {
        private SoliSocialDbContext _context;
        public HMACAuthorization(SoliSocialDbContext context)
        {
            _context = context;
        }

        public bool Chalenge(ApiAuthHeader authHeader, string method, string body = "")
        {

            var appData = _context.TdApiClient.FirstOrDefault(ac => ac.Id == authHeader.AppId);

            if (appData == null)
                return false;

            string key = appData.Key;

            //string stripedBody = body.Replace("\\", "");
            byte[] content = Encoding.UTF8.GetBytes(body);
            MD5 md5 = MD5.Create();
            byte[] requestContentHash = md5.ComputeHash(content);

            var calcSignature = "";
            if (body == "")
                calcSignature = authHeader.AppId + method + authHeader.Timestamp + authHeader.Nonce;
            else
                calcSignature = authHeader.AppId + method + authHeader.Timestamp + authHeader.Nonce + ByteArrayToString(requestContentHash);
            byte[] calcSignatureBa = Encoding.UTF8.GetBytes(calcSignature);
            byte[] secretKeyByteArray = Encoding.UTF8.GetBytes(key);

            using (HMACSHA256 hmac = new HMACSHA256(secretKeyByteArray))
            {
                byte[] signatureBytes = hmac.ComputeHash(calcSignatureBa);
                string signatureHex = ByteArrayToString(signatureBytes);

                if (signatureHex == authHeader.Signature)
                    return true;
                return false;
            }
        }

        public static string ReadBody(HttpContext httpContext)
        {
            byte[] body = new byte[httpContext.Request.Body.Length];
            int bodyLength = int.Parse(httpContext.Request.Body.Length.ToString());

            httpContext.Request.Body.Seek(0, System.IO.SeekOrigin.Begin);
            httpContext.Request.Body.Read(body, 0, bodyLength);

            return Encoding.UTF8.GetString(body);
        }

        public static ApiAuthHeader GetApiAuthHeader(HttpContext httpContext)
        {
            //START Grab API Auth Header
            var req = httpContext.Request;

            var apiAuthHeaderStr = req.Headers.FirstOrDefault(h => h.Key == "api_auth").Value.FirstOrDefault();

            if (apiAuthHeaderStr == null)
                return null;

            apiAuthHeaderStr = apiAuthHeaderStr.Replace("\\", "");

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

        private static string ByteArrayToString(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", "").ToLower();
        }
    }
}
