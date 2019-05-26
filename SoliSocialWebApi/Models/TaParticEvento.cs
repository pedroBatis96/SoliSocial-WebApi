using System;
using System.Collections.Generic;

namespace SoliSocialWebApi.Models
{
    public partial class TaParticEvento
    {
        public long EventId { get; set; }
        public Guid UserId { get; set; }
        public long TarefaId { get; set; }
        public bool Staff { get; set; }

        public virtual TdEvento Event { get; set; }
        public virtual TdTarefas Tarefa { get; set; }
        public virtual TdUsers User { get; set; }
    }
}
