using System;
using System.Collections.Generic;

namespace SoliSocialWebApi.Models
{
    public partial class TdUsers
    {
        public TdUsers()
        {
            TaParticEvento = new HashSet<TaParticEvento>();
            TaStaffInstituicao = new HashSet<TaStaffInstituicao>();
            TaUserInstituicaoBlock = new HashSet<TaUserInstituicaoBlock>();
            TaUserInstituicaoFav = new HashSet<TaUserInstituicaoFav>();
            TaUserRoles = new HashSet<TaUserRoles>();
            TdEvento = new HashSet<TdEvento>();
            TdInstituicao = new HashSet<TdInstituicao>();
            TdNoticias = new HashSet<TdNoticias>();
        }

        public string Id { get; set; }
        public string Email { get; set; }
        public sbyte EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string Username { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public string Genero { get; set; }
        public string Bio { get; set; }
        public string Imagem { get; set; }
        public string Phonenumber { get; set; }
        public sbyte PhonenumberConfirmed { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string ConcurrencyStamp { get; set; }

        public virtual ICollection<TaParticEvento> TaParticEvento { get; set; }
        public virtual ICollection<TaStaffInstituicao> TaStaffInstituicao { get; set; }
        public virtual ICollection<TaUserInstituicaoBlock> TaUserInstituicaoBlock { get; set; }
        public virtual ICollection<TaUserInstituicaoFav> TaUserInstituicaoFav { get; set; }
        public virtual ICollection<TaUserRoles> TaUserRoles { get; set; }
        public virtual ICollection<TdEvento> TdEvento { get; set; }
        public virtual ICollection<TdInstituicao> TdInstituicao { get; set; }
        public virtual ICollection<TdNoticias> TdNoticias { get; set; }
    }
}
