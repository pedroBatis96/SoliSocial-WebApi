using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoliSocialWebApi.ViewModels.UserManagement
{
    public class GetUserInfo
    {
        public string UserId { get; set; }
    }

    public class GetUserFeed
    {
        public string InstId { get; set; }
    }
}
