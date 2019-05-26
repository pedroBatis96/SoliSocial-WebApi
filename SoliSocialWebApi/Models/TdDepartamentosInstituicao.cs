using System;
using System.Collections.Generic;

namespace SoliSocialWebApi.Models
{
    public partial class TdDepartamentosInstituicao
    {
        public TdDepartamentosInstituicao()
        {
            TaStaffInstituicao = new HashSet<TaStaffInstituicao>();
        }

        public long Id { get; set; }
        public Guid InstituicaoId { get; set; }
        public long? IdPai { get; set; }
        public string Descricao { get; set; }

        public virtual TdInstituicao Instituicao { get; set; }
        public virtual ICollection<TaStaffInstituicao> TaStaffInstituicao { get; set; }
    }
}
