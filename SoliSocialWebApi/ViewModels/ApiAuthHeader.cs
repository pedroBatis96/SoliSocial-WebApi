using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoliSocialWebApi.ViewModels
{
    public class ApiAuthHeader
    {
        public string Timestamp { get; set; }
        public string Nonce { get; set; }
        public string Signature { get; set; }
        public string AppId { get; set; }
    }
}
