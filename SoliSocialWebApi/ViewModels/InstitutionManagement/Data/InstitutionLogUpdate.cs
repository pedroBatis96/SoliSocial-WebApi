using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoliSocialWebApi.ViewModels.InstitutionManagement
{
    public class InstitutionLogUpdate
    {
        public Guid Id { get; set; }
        public string Logo { get; set; }
        public string Password { get; set; }
    }
}
