using System;
using System.Collections.Generic;

namespace SoliSocialWebApi.Models
{
    public partial class TaStaffInstituicao
    {
        public string UserId { get; set; }
        public string InstituicaoId { get; set; }
        public long DepartamentoId { get; set; }
        public DateTime DataEntrada { get; set; }

        public virtual TdDepartamentosInstituicao Departamento { get; set; }
        public virtual TdInstituicao Instituicao { get; set; }
        public virtual TdUsers User { get; set; }
    }
}
