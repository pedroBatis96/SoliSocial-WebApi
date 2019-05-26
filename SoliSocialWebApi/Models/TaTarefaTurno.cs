using System;
using System.Collections.Generic;

namespace SoliSocialWebApi.Models
{
    public partial class TaTarefaTurno
    {
        public long TarefaId { get; set; }
        public long TurnoId { get; set; }

        public virtual TdTarefas Tarefa { get; set; }
        public virtual TdTurno Turno { get; set; }
    }
}
