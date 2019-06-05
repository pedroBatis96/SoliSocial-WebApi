using System;
using System.Collections.Generic;

namespace SoliSocialWebApi.Models
{
    public partial class TaParticEvento
    {
        public long EventId { get; set; }
        public string UserId { get; set; }
        public long TarefaId { get; set; }
        public sbyte Staff { get; set; }

        public virtual TdEvento Event { get; set; }
        public virtual TdTarefas Tarefa { get; set; }
        public virtual TdUsers User { get; set; }
    }
}
