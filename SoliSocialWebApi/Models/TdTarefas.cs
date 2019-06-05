using System;
using System.Collections.Generic;

namespace SoliSocialWebApi.Models
{
    public partial class TdTarefas
    {
        public TdTarefas()
        {
            TaParticEvento = new HashSet<TaParticEvento>();
            TaTarefaTurno = new HashSet<TaTarefaTurno>();
        }

        public long Id { get; set; }
        public string InstituicaoId { get; set; }
        public int? NumParticMax { get; set; }
        public string Descricao { get; set; }
        public sbyte Turnos { get; set; }

        public virtual TdInstituicao Instituicao { get; set; }
        public virtual ICollection<TaParticEvento> TaParticEvento { get; set; }
        public virtual ICollection<TaTarefaTurno> TaTarefaTurno { get; set; }
    }
}
