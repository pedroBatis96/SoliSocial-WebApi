using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SoliSocialWebApi.Models
{
    public partial class SoliSocialDbContext : DbContext
    {
        public SoliSocialDbContext()
        {
        }

        public SoliSocialDbContext(DbContextOptions<SoliSocialDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TaEventoImagem> TaEventoImagem { get; set; }
        public virtual DbSet<TaInstDoc> TaInstDoc { get; set; }
        public virtual DbSet<TaInstituicaoImagem> TaInstituicaoImagem { get; set; }
        public virtual DbSet<TaNoticiaImagens> TaNoticiaImagens { get; set; }
        public virtual DbSet<TaParticEvento> TaParticEvento { get; set; }
        public virtual DbSet<TaStaffInstituicao> TaStaffInstituicao { get; set; }
        public virtual DbSet<TaTarefaTurno> TaTarefaTurno { get; set; }
        public virtual DbSet<TaUserInstituicaoBlock> TaUserInstituicaoBlock { get; set; }
        public virtual DbSet<TaUserInstituicaoFav> TaUserInstituicaoFav { get; set; }
        public virtual DbSet<TaUserRoles> TaUserRoles { get; set; }
        public virtual DbSet<TdApiClient> TdApiClient { get; set; }
        public virtual DbSet<TdDepartamentosInstituicao> TdDepartamentosInstituicao { get; set; }
        public virtual DbSet<TdDocSupp> TdDocSupp { get; set; }
        public virtual DbSet<TdEvento> TdEvento { get; set; }
        public virtual DbSet<TdInstituicao> TdInstituicao { get; set; }
        public virtual DbSet<TdNoticias> TdNoticias { get; set; }
        public virtual DbSet<TdTarefas> TdTarefas { get; set; }
        public virtual DbSet<TdTemplates> TdTemplates { get; set; }
        public virtual DbSet<TdTurno> TdTurno { get; set; }
        public virtual DbSet<TdUserRoles> TdUserRoles { get; set; }
        public virtual DbSet<TdUsers> TdUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost;Database=SoliSocial;User Id=SoliSocialUser;Password=taranta22");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<TaEventoImagem>(entity =>
            {
                entity.ToTable("TA_EVENTO_IMAGEM");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.EventoId).HasColumnName("EVENTO_ID");

                entity.Property(e => e.Imagem)
                    .IsRequired()
                    .HasColumnName("IMAGEM");

                entity.HasOne(d => d.Evento)
                    .WithMany(p => p.TaEventoImagem)
                    .HasForeignKey(d => d.EventoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TA_EVENTO_IMAGEM_TD_EVENTO");
            });

            modelBuilder.Entity<TaInstDoc>(entity =>
            {
                entity.HasKey(e => new { e.InstId, e.IdDoc });

                entity.ToTable("TA_INST_DOC");

                entity.Property(e => e.InstId).HasColumnName("INST_ID");

                entity.Property(e => e.IdDoc).HasColumnName("ID_DOC");

                entity.HasOne(d => d.IdDocNavigation)
                    .WithMany(p => p.TaInstDoc)
                    .HasForeignKey(d => d.IdDoc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TA_INST_DOC_TD_DOC_SUPP");

                entity.HasOne(d => d.Inst)
                    .WithMany(p => p.TaInstDoc)
                    .HasForeignKey(d => d.InstId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TA_INST_DOC_TD_INSTITUICAO");
            });

            modelBuilder.Entity<TaInstituicaoImagem>(entity =>
            {
                entity.ToTable("TA_INSTITUICAO_IMAGEM");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasColumnName("IMAGE");

                entity.Property(e => e.InstituicaoId).HasColumnName("INSTITUICAO_ID");

                entity.HasOne(d => d.Instituicao)
                    .WithMany(p => p.TaInstituicaoImagem)
                    .HasForeignKey(d => d.InstituicaoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TA_INSTITUICAO_IMAGENS_TD_INSTITUICAO");
            });

            modelBuilder.Entity<TaNoticiaImagens>(entity =>
            {
                entity.ToTable("TA_NOTICIA_IMAGENS");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasColumnName("IMAGE");

                entity.Property(e => e.NoticiaId).HasColumnName("NOTICIA_ID");

                entity.HasOne(d => d.Noticia)
                    .WithMany(p => p.TaNoticiaImagens)
                    .HasForeignKey(d => d.NoticiaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TA_NOTICIA_IMAGENS_TD_NOTICIA");
            });

            modelBuilder.Entity<TaParticEvento>(entity =>
            {
                entity.HasKey(e => new { e.EventId, e.UserId, e.TarefaId });

                entity.ToTable("TA_PARTIC_EVENTO");

                entity.Property(e => e.EventId).HasColumnName("EVENT_ID");

                entity.Property(e => e.UserId).HasColumnName("USER_ID");

                entity.Property(e => e.TarefaId).HasColumnName("TAREFA_ID");

                entity.Property(e => e.Staff).HasColumnName("STAFF");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.TaParticEvento)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TA_PARTIC_EVENTO_TD_EVENTO");

                entity.HasOne(d => d.Tarefa)
                    .WithMany(p => p.TaParticEvento)
                    .HasForeignKey(d => d.TarefaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TA_PARTIC_EVENTO_TD_TAREFA");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TaParticEvento)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TA_PARTIC_EVENTO_TD_USERS");
            });

            modelBuilder.Entity<TaStaffInstituicao>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.InstituicaoId, e.DepartamentoId });

                entity.ToTable("TA_STAFF_INSTITUICAO");

                entity.Property(e => e.UserId).HasColumnName("USER_ID");

                entity.Property(e => e.InstituicaoId).HasColumnName("INSTITUICAO_ID");

                entity.Property(e => e.DepartamentoId).HasColumnName("DEPARTAMENTO_ID");

                entity.Property(e => e.DataEntrada)
                    .HasColumnName("DATA_ENTRADA")
                    .HasColumnType("date");

                entity.HasOne(d => d.Departamento)
                    .WithMany(p => p.TaStaffInstituicao)
                    .HasForeignKey(d => d.DepartamentoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TA_STAFF_INSTITUICAO_TD_DEPARTAMENTO");

                entity.HasOne(d => d.Instituicao)
                    .WithMany(p => p.TaStaffInstituicao)
                    .HasForeignKey(d => d.InstituicaoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TA_STAFF_INSTITUICAO_TD_INSTITUICAO");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TaStaffInstituicao)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TA_STAFF_INSTITUICAO_TD_USER");
            });

            modelBuilder.Entity<TaTarefaTurno>(entity =>
            {
                entity.HasKey(e => new { e.TarefaId, e.TurnoId });

                entity.ToTable("TA_TAREFA_TURNO");

                entity.Property(e => e.TarefaId).HasColumnName("TAREFA_ID");

                entity.Property(e => e.TurnoId).HasColumnName("TURNO_ID");

                entity.HasOne(d => d.Tarefa)
                    .WithMany(p => p.TaTarefaTurno)
                    .HasForeignKey(d => d.TarefaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TA_TAREFA_TURNO_TD_TAREFA");

                entity.HasOne(d => d.Turno)
                    .WithMany(p => p.TaTarefaTurno)
                    .HasForeignKey(d => d.TurnoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TA_TAREFA_TURNO_TD_TURNO");
            });

            modelBuilder.Entity<TaUserInstituicaoBlock>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.InstituicaoId });

                entity.ToTable("TA_USER_INSTITUICAO_BLOCK");

                entity.Property(e => e.UserId).HasColumnName("USER_ID");

                entity.Property(e => e.InstituicaoId).HasColumnName("INSTITUICAO_ID");

                entity.HasOne(d => d.Instituicao)
                    .WithMany(p => p.TaUserInstituicaoBlock)
                    .HasForeignKey(d => d.InstituicaoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TA_USER_INSTITUICAO_BLOCK_TD_INSTITUICAO");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TaUserInstituicaoBlock)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TA_USER_INSTITUICAO_BLOCK_TD_USER");
            });

            modelBuilder.Entity<TaUserInstituicaoFav>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.InstituicaoId });

                entity.ToTable("TA_USER_INSTITUICAO_FAV");

                entity.Property(e => e.UserId).HasColumnName("USER_ID");

                entity.Property(e => e.InstituicaoId).HasColumnName("INSTITUICAO_ID");

                entity.HasOne(d => d.Instituicao)
                    .WithMany(p => p.TaUserInstituicaoFav)
                    .HasForeignKey(d => d.InstituicaoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TA_USER_INSTITUICAO_FAV_TD_INSTITUICAO");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TaUserInstituicaoFav)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TA_USER_INSTITUICAO_FAV_TD_USER");
            });

            modelBuilder.Entity<TaUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PK_UserGroup")
                    .ForSqlServerIsClustered(false);

                entity.ToTable("TA_USER_ROLES");

                entity.Property(e => e.UserId).HasColumnName("USER_ID");

                entity.Property(e => e.RoleId).HasColumnName("ROLE_ID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TaUserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TA_USER_ROLES_TD_USER_ROLES");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TaUserRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TA_USER_ROLES_TD_USER");
            });

            modelBuilder.Entity<TdApiClient>(entity =>
            {
                entity.ToTable("TD_API_CLIENT");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasColumnName("KEY")
                    .IsUnicode(false);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("NOME")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TdDepartamentosInstituicao>(entity =>
            {
                entity.ToTable("TD_DEPARTAMENTOS_INSTITUICAO");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Descricao)
                    .HasColumnName("DESCRICAO")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdPai).HasColumnName("ID_PAI");

                entity.Property(e => e.InstituicaoId).HasColumnName("INSTITUICAO_ID");

                entity.HasOne(d => d.Instituicao)
                    .WithMany(p => p.TdDepartamentosInstituicao)
                    .HasForeignKey(d => d.InstituicaoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TD_DEPARTAMENTOS_INSTITUICAO_TD_INSTITUICAO");
            });

            modelBuilder.Entity<TdDocSupp>(entity =>
            {
                entity.ToTable("TD_DOC_SUPP");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Doc)
                    .IsRequired()
                    .HasColumnName("DOC");
            });

            modelBuilder.Entity<TdEvento>(entity =>
            {
                entity.ToTable("TD_EVENTO");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CriadoPor).HasColumnName("CRIADO_POR");

                entity.Property(e => e.DataAlteracao)
                    .HasColumnName("DATA_ALTERACAO")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DATA_CRIACAO")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataFim)
                    .HasColumnName("DATA_FIM")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataInicio)
                    .HasColumnName("DATA_INICIO")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasColumnName("DESCRICAO")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.InstId).HasColumnName("INST_ID");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("NOME")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.NumParticipantesMax).HasColumnName("NUM_PARTICIPANTES_MAX");

                entity.Property(e => e.NumStaffMaximo).HasColumnName("NUM_STAFF_MAXIMO");

                entity.Property(e => e.Pagina)
                    .HasColumnName("PAGINA")
                    .IsUnicode(false);

                entity.HasOne(d => d.CriadoPorNavigation)
                    .WithMany(p => p.TdEvento)
                    .HasForeignKey(d => d.CriadoPor)
                    .HasConstraintName("FK_TD_EVENTO_TD_USER");

                entity.HasOne(d => d.Inst)
                    .WithMany(p => p.TdEvento)
                    .HasForeignKey(d => d.InstId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TD_EVENTO_TD_INSTITUICAO");
            });

            modelBuilder.Entity<TdInstituicao>(entity =>
            {
                entity.ToTable("TD_INSTITUICAO");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Acronimo)
                    .IsRequired()
                    .HasColumnName("ACRONIMO")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoPostal)
                    .IsRequired()
                    .HasColumnName("CODIGO_POSTAL")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CriadoPor).HasColumnName("CRIADO_POR");

                entity.Property(e => e.DataAlteracao)
                    .HasColumnName("DATA_ALTERACAO")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DATA_CRIACAO")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasColumnName("DESCRICAO")
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("EMAIL")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Iban)
                    .HasColumnName("IBAN")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Logo)
                    .IsRequired()
                    .HasColumnName("LOGO")
                    .IsUnicode(false);

                entity.Property(e => e.Morada)
                    .IsRequired()
                    .HasColumnName("MORADA")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Nif)
                    .HasColumnName("NIF")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("NOME")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Pagina)
                    .HasColumnName("PAGINA")
                    .IsUnicode(false);

                entity.Property(e => e.Phonenumber)
                    .IsRequired()
                    .HasColumnName("PHONENUMBER")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.CriadoPorNavigation)
                    .WithMany(p => p.TdInstituicao)
                    .HasForeignKey(d => d.CriadoPor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TD_INSTITUICAO_TD_USERS");
            });

            modelBuilder.Entity<TdNoticias>(entity =>
            {
                entity.ToTable("TD_NOTICIAS");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CriadoPor).HasColumnName("CRIADO_POR");

                entity.Property(e => e.DataAlteracao)
                    .HasColumnName("DATA_ALTERACAO")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DATA_CRIACAO")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasColumnName("DESCRICAO")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.EventoId).HasColumnName("EVENTO_ID");

                entity.Property(e => e.InstId).HasColumnName("INST_ID");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("NOME")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Pagina)
                    .IsRequired()
                    .HasColumnName("PAGINA")
                    .IsUnicode(false);

                entity.HasOne(d => d.Evento)
                    .WithMany(p => p.TdNoticias)
                    .HasForeignKey(d => d.EventoId)
                    .HasConstraintName("FK_TD_NOTICIAS_TD_EVENTO");

                entity.HasOne(d => d.Inst)
                    .WithMany(p => p.TdNoticias)
                    .HasForeignKey(d => d.InstId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TD_NOTICIAS_TD_INSTITUICAO");
            });

            modelBuilder.Entity<TdTarefas>(entity =>
            {
                entity.ToTable("TD_TAREFAS");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasColumnName("DESCRICAO")
                    .IsUnicode(false);

                entity.Property(e => e.InstituicaoId).HasColumnName("INSTITUICAO_ID");

                entity.Property(e => e.NumParticMax).HasColumnName("NUM_PARTIC_MAX");

                entity.Property(e => e.Turnos).HasColumnName("TURNOS");

                entity.HasOne(d => d.Instituicao)
                    .WithMany(p => p.TdTarefas)
                    .HasForeignKey(d => d.InstituicaoId)
                    .HasConstraintName("FK_TD_TAREFAS_TD_INSTITUICAO");
            });

            modelBuilder.Entity<TdTemplates>(entity =>
            {
                entity.ToTable("TD_TEMPLATES");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.DataAlteracao)
                    .HasColumnName("DATA_ALTERACAO")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DATA_CRIACAO")
                    .HasColumnType("datetime");

                entity.Property(e => e.Evento).HasColumnName("EVENTO");

                entity.Property(e => e.Noticia).HasColumnName("NOTICIA");

                entity.Property(e => e.Pagina)
                    .IsRequired()
                    .HasColumnName("PAGINA")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TdTurno>(entity =>
            {
                entity.ToTable("TD_TURNO");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.HoraFinal)
                    .HasColumnName("HORA_FINAL")
                    .HasColumnType("datetime");

                entity.Property(e => e.HoraInicial)
                    .HasColumnName("HORA_INICIAL")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<TdUserRoles>(entity =>
            {
                entity.ToTable("TD_USER_ROLES");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ConcurrencyStamp)
                    .IsRequired()
                    .HasColumnName("CONCURRENCY_STAMP");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.NormalizedName)
                    .IsRequired()
                    .HasColumnName("NORMALIZED_NAME")
                    .HasMaxLength(256)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TdUsers>(entity =>
            {
                entity.ToTable("TD_USERS");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Age).HasColumnName("AGE");

                entity.Property(e => e.Bio)
                    .HasColumnName("BIO")
                    .IsUnicode(false);

                entity.Property(e => e.ConcurrencyStamp)
                    .IsRequired()
                    .HasColumnName("CONCURRENCY_STAMP");

                entity.Property(e => e.DataAlteracao)
                    .HasColumnName("DATA_ALTERACAO")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DATA_CRIACAO")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateOfBirth)
                    .HasColumnName("DATE_OF_BIRTH")
                    .HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("EMAIL")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.EmailConfirmed).HasColumnName("EMAIL_CONFIRMED");

                entity.Property(e => e.Genero)
                    .IsRequired()
                    .HasColumnName("GENERO")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Imagem)
                    .HasColumnName("IMAGEM")
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.NormalizedName)
                    .IsRequired()
                    .HasColumnName("NORMALIZED_NAME")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasColumnName("PASSWORD_HASH");

                entity.Property(e => e.Phonenumber)
                    .HasColumnName("PHONENUMBER")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PhonenumberConfirmed).HasColumnName("PHONENUMBER_CONFIRMED");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("USERNAME")
                    .HasMaxLength(256)
                    .IsUnicode(false);
            });
        }
    }
}
