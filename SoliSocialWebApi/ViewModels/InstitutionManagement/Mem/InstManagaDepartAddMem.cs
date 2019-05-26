using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoliSocialWebApi.ViewModels.InstitutionManagement.Mem
{
    public class InstManagaDepartAddMem
    {
        public Guid IdUser { get; set; }
        public Guid IdInst { get; set; }
        public long IdDepart { get; set; }
    }
    public class InstManagaDepartRemMem
    {
        public Guid IdUser { get; set; }
        public Guid IdInst { get; set; }
        public long IdDepart { get; set; }
    }

    public class InstManagDepartAddDep
    {
        public Guid IdInst { get; set; }
        public string Descricao { get; set; }

    }

    public class InstManagDepartRemDep
    {
        public Guid idInst { get; set; }
        public long IdDepart { get; set; }

    }
}
