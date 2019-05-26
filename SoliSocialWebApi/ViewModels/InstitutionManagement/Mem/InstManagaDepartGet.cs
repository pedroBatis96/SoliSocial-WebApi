using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoliSocialWebApi.ViewModels.InstitutionManagement.Mem
{
    public class InstManagaDepartGet
    {
        public Guid Id { get; set; }
    }

    public class InstManagDepartGetCurrent
    {
        public Guid Id { get; set; }
        public long DepId { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
    }
}
