using System;
using System.Collections.Generic;

namespace SoliSocialWebApi.Models
{
    public partial class TdTurno
    {
        public TdTurno()
        {
            TaTarefaTurno = new HashSet<TaTarefaTurno>();
        }

        public long Id { get; set; }
        public DateTime HoraInicial { get; set; }
        public DateTime? HoraFinal { get; set; }

        public virtual ICollection<TaTarefaTurno> TaTarefaTurno { get; set; }
    }
}
