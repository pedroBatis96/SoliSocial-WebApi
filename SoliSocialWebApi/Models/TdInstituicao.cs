using System;
using System.Collections.Generic;

namespace SoliSocialWebApi.Models
{
    public partial class TdInstituicao
    {
        public TdInstituicao()
        {
            TaInstDoc = new HashSet<TaInstDoc>();
            TaInstituicaoImagem = new HashSet<TaInstituicaoImagem>();
            TaStaffInstituicao = new HashSet<TaStaffInstituicao>();
            TaUserInstituicaoBlock = new HashSet<TaUserInstituicaoBlock>();
            TaUserInstituicaoFav = new HashSet<TaUserInstituicaoFav>();
            TdDepartamentosInstituicao = new HashSet<TdDepartamentosInstituicao>();
            TdEvento = new HashSet<TdEvento>();
            TdNoticias = new HashSet<TdNoticias>();
            TdTarefas = new HashSet<TdTarefas>();
        }

        public string Id { get; set; }
        public string Nome { get; set; }
        public string Acronimo { get; set; }
        public string Descricao { get; set; }
        public string Morada { get; set; }
        public string Email { get; set; }
        public string Phonenumber { get; set; }
        public string Pagina { get; set; }
        public string Iban { get; set; }
        public string Nif { get; set; }
        public string CodigoPostal { get; set; }
        public string Logo { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string CriadoPor { get; set; }

        public virtual TdUsers CriadoPorNavigation { get; set; }
        public virtual ICollection<TaInstDoc> TaInstDoc { get; set; }
        public virtual ICollection<TaInstituicaoImagem> TaInstituicaoImagem { get; set; }
        public virtual ICollection<TaStaffInstituicao> TaStaffInstituicao { get; set; }
        public virtual ICollection<TaUserInstituicaoBlock> TaUserInstituicaoBlock { get; set; }
        public virtual ICollection<TaUserInstituicaoFav> TaUserInstituicaoFav { get; set; }
        public virtual ICollection<TdDepartamentosInstituicao> TdDepartamentosInstituicao { get; set; }
        public virtual ICollection<TdEvento> TdEvento { get; set; }
        public virtual ICollection<TdNoticias> TdNoticias { get; set; }
        public virtual ICollection<TdTarefas> TdTarefas { get; set; }
    }
}
