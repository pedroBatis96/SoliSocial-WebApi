using SoliSocialWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoliSocialWebApi.Services.Abstraction
{
    public interface IAuthService
    {
        string HashPassword(string password);
        bool VerifyPassword(string actualPassword, string hashedPassword);
        AuthData GetAuthData(string id);
    }
}
