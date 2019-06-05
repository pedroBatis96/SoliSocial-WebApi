using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoliSocialWebApi.ViewModels.InstitutionManagement.Mem
{
    public class InstManagaDepartAddMem
    {
        public string IdUser { get; set; }
        public string IdInst { get; set; }
        public long IdDepart { get; set; }
    }
    public class InstManagaDepartRemMem
    {
        public string IdUser { get; set; }
        public string IdInst { get; set; }
        public long IdDepart { get; set; }
    }

    public class InstManagDepartAddDep
    {
        public string IdInst { get; set; }
        public string Descricao { get; set; }

    }

    public class InstManagDepartRemDep
    {
        public string idInst { get; set; }
        public long IdDepart { get; set; }

    }
}
