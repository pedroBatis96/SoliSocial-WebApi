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
                optionsBuilder.UseMySql("Server=pcdev.pt;Database=admin_solisocial;User Id=admin_solisocial;Password=wt!Rh944");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaEventoImagem>(entity =>
            {
                entity.ToTable("TA_EVENTO_IMAGEM");

                entity.HasIndex(e => e.EventoId)
                    .HasName("FK_TA_EVENTO_IMG_TD_EVENTO_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.EventoId)
                    .HasColumnName("EVENTO_ID")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Imagem)
                    .IsRequired()
                    .HasColumnName("IMAGEM");

                entity.HasOne(d => d.Evento)
                    .WithMany(p => p.TaEventoImagem)
                    .HasForeignKey(d => d.EventoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TA_EVENTO_IMG_TD_EVENTO");
            });

            modelBuilder.Entity<TaInstDoc>(entity =>
            {
                entity.HasKey(e => new { e.InstId, e.IdDoc })
                    .HasName("PRIMARY");

                entity.ToTable("TA_INST_DOC");

                entity.HasIndex(e => e.IdDoc)
                    .HasName("TA_INST_DOC_DOC");

                entity.Property(e => e.InstId)
                    .HasColumnName("INST_ID")
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.IdDoc)
                    .HasColumnName("ID_DOC")
                    .HasColumnType("bigint(20)");

                entity.HasOne(d => d.IdDocNavigation)
                    .WithMany(p => p.TaInstDoc)
                    .HasForeignKey(d => d.IdDoc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TA_INST_DOC_DOC");

                entity.HasOne(d => d.Inst)
                    .WithMany(p => p.TaInstDoc)
                    .HasForeignKey(d => d.InstId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TA_INST_DOC_INST");
            });

            modelBuilder.Entity<TaInstituicaoImagem>(entity =>
            {
                entity.ToTable("TA_INSTITUICAO_IMAGEM");

                entity.HasIndex(e => e.InstituicaoId)
                    .HasName("TA_INST_IMG_INST_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasColumnName("IMAGE");

                entity.Property(e => e.InstituicaoId)
                    .IsRequired()
                    .HasColumnName("INSTITUICAO_ID")
                    .HasColumnType("varchar(64)");

                entity.HasOne(d => d.Instituicao)
                    .WithMany(p => p.TaInstituicaoImagem)
                    .HasForeignKey(d => d.InstituicaoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TA_INST_IMG_INST");
            });

            modelBuilder.Entity<TaNoticiaImagens>(entity =>
            {
                entity.ToTable("TA_NOTICIA_IMAGENS");

                entity.HasIndex(e => e.NoticiaId)
                    .HasName("FK_TA_NOTICIA_IMAGENS_TD_NOTICIA");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasColumnName("IMAGE");

                entity.Property(e => e.NoticiaId)
                    .HasColumnName("NOTICIA_ID")
                    .HasColumnType("bigint(20)");

                entity.HasOne(d => d.Noticia)
                    .WithMany(p => p.TaNoticiaImagens)
                    .HasForeignKey(d => d.NoticiaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TA_NOTICIA_IMAGENS_TD_NOTICIA");
            });

            modelBuilder.Entity<TaParticEvento>(entity =>
            {
                entity.HasKey(e => new { e.EventId, e.UserId, e.TarefaId })
                    .HasName("PRIMARY");

                entity.ToTable("TA_PARTIC_EVENTO");

                entity.HasIndex(e => e.TarefaId)
                    .HasName("TA_PARTIC_EVENT_TAREF");

                entity.HasIndex(e => e.UserId)
                    .HasName("TA_PARTIC_EVENT_USER_idx");

                entity.Property(e => e.EventId)
                    .HasColumnName("EVENT_ID")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.TarefaId)
                    .HasColumnName("TAREFA_ID")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Staff)
                    .HasColumnName("STAFF")
                    .HasColumnType("tinyint(1)");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.TaParticEvento)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TA_PARTIC_EVENT_EVENT");

                entity.HasOne(d => d.Tarefa)
                    .WithMany(p => p.TaParticEvento)
                    .HasForeignKey(d => d.TarefaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TA_PARTIC_EVENT_TAREF");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TaParticEvento)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TA_PARTIC_EVENT_USER");
            });

            modelBuilder.Entity<TaStaffInstituicao>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.InstituicaoId, e.DepartamentoId })
                    .HasName("PRIMARY");

                entity.ToTable("TA_STAFF_INSTITUICAO");

                entity.HasIndex(e => e.DepartamentoId)
                    .HasName("FK_TA_STAFF_INSTITUICAO_TD_DEPARTAMENTO_idx");

                entity.HasIndex(e => e.InstituicaoId)
                    .HasName("FK_TA_STAFF_INSTITUICAO_TD_INSTITUICAO_idx");

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.InstituicaoId)
                    .HasColumnName("INSTITUICAO_ID")
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.DepartamentoId)
                    .HasColumnName("DEPARTAMENTO_ID")
                    .HasColumnType("bigint(20)");

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
                entity.HasKey(e => new { e.TarefaId, e.TurnoId })
                    .HasName("PRIMARY");

                entity.ToTable("TA_TAREFA_TURNO");

                entity.HasIndex(e => e.TurnoId)
                    .HasName("FK_TA_TAREFA_TURNO_TD_TURNO");

                entity.Property(e => e.TarefaId)
                    .HasColumnName("TAREFA_ID")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.TurnoId)
                    .HasColumnName("TURNO_ID")
                    .HasColumnType("bigint(20)");

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
                entity.HasKey(e => new { e.UserId, e.InstituicaoId })
                    .HasName("PRIMARY");

                entity.ToTable("TA_USER_INSTITUICAO_BLOCK");

                entity.HasIndex(e => e.InstituicaoId)
                    .HasName("TA_USER_BLOCK_INST_idx");

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.InstituicaoId)
                    .HasColumnName("INSTITUICAO_ID")
                    .HasColumnType("varchar(64)");

                entity.HasOne(d => d.Instituicao)
                    .WithMany(p => p.TaUserInstituicaoBlock)
                    .HasForeignKey(d => d.InstituicaoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TA_USER_BLOCK_INST");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TaUserInstituicaoBlock)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TA_USER_BLOCK_USER");
            });

            modelBuilder.Entity<TaUserInstituicaoFav>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.InstituicaoId })
                    .HasName("PRIMARY");

                entity.ToTable("TA_USER_INSTITUICAO_FAV");

                entity.HasIndex(e => e.InstituicaoId)
                    .HasName("TA_USER_FAV_INST_idx");

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.InstituicaoId)
                    .HasColumnName("INSTITUICAO_ID")
                    .HasColumnType("varchar(64)");

                entity.HasOne(d => d.Instituicao)
                    .WithMany(p => p.TaUserInstituicaoFav)
                    .HasForeignKey(d => d.InstituicaoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TA_USER_FAV_INST");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TaUserInstituicaoFav)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TA_USER_FAV_USER");
            });

            modelBuilder.Entity<TaUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PRIMARY");

                entity.ToTable("TA_USER_ROLES");

                entity.HasIndex(e => e.RoleId)
                    .HasName("FK_TA_USER_ROLES_TD_USER_ROLES_idx");

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.RoleId)
                    .HasColumnName("ROLE_ID")
                    .HasColumnType("varchar(64)");

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
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasColumnName("KEY")
                    .HasColumnType("longtext");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("NOME")
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<TdDepartamentosInstituicao>(entity =>
            {
                entity.ToTable("TD_DEPARTAMENTOS_INSTITUICAO");

                entity.HasIndex(e => e.InstituicaoId)
                    .HasName("FK_DEPARTAMENTOS_INST_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Descricao)
                    .HasColumnName("DESCRICAO")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.IdPai)
                    .HasColumnName("ID_PAI")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.InstituicaoId)
                    .IsRequired()
                    .HasColumnName("INSTITUICAO_ID")
                    .HasColumnType("varchar(64)");

                entity.HasOne(d => d.Instituicao)
                    .WithMany(p => p.TdDepartamentosInstituicao)
                    .HasForeignKey(d => d.InstituicaoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DEPARTAMENTOS_INST");
            });

            modelBuilder.Entity<TdDocSupp>(entity =>
            {
                entity.ToTable("TD_DOC_SUPP");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Doc)
                    .IsRequired()
                    .HasColumnName("DOC");
            });

            modelBuilder.Entity<TdEvento>(entity =>
            {
                entity.ToTable("TD_EVENTO");

                entity.HasIndex(e => e.CriadoPor)
                    .HasName("FK_TD_EVENTO_TD_USER_idx");

                entity.HasIndex(e => e.InstId)
                    .HasName("FK_TD_EVENTO_TD_INST_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.CriadoPor)
                    .HasColumnName("CRIADO_POR")
                    .HasColumnType("varchar(64)");

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
                    .HasColumnType("varchar(256)");

                entity.Property(e => e.InstId)
                    .IsRequired()
                    .HasColumnName("INST_ID")
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("NOME")
                    .HasColumnType("varchar(256)");

                entity.Property(e => e.NumParticipantesMax)
                    .HasColumnName("NUM_PARTICIPANTES_MAX")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NumStaffMaximo)
                    .HasColumnName("NUM_STAFF_MAXIMO")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Pagina)
                    .HasColumnName("PAGINA")
                    .HasColumnType("longtext");

                entity.HasOne(d => d.CriadoPorNavigation)
                    .WithMany(p => p.TdEvento)
                    .HasForeignKey(d => d.CriadoPor)
                    .HasConstraintName("FK_TD_EVENTO_TD_USER");

                entity.HasOne(d => d.Inst)
                    .WithMany(p => p.TdEvento)
                    .HasForeignKey(d => d.InstId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TD_EVENTO_TD_INST");
            });

            modelBuilder.Entity<TdInstituicao>(entity =>
            {
                entity.ToTable("TD_INSTITUICAO");

                entity.HasIndex(e => e.CriadoPor)
                    .HasName("FK_TD_INSTITUICAO_TD_USERS_idx");

                entity.HasIndex(e => e.Id)
                    .HasName("ID_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.Acronimo)
                    .IsRequired()
                    .HasColumnName("ACRONIMO")
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.CodigoPostal)
                    .IsRequired()
                    .HasColumnName("CODIGO_POSTAL")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.CriadoPor)
                    .IsRequired()
                    .HasColumnName("CRIADO_POR")
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.DataAlteracao)
                    .HasColumnName("DATA_ALTERACAO")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DATA_CRIACAO")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasColumnName("DESCRICAO")
                    .HasColumnType("longtext");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("EMAIL")
                    .HasColumnType("varchar(256)");

                entity.Property(e => e.Iban)
                    .HasColumnName("IBAN")
                    .HasColumnType("varchar(40)");

                entity.Property(e => e.Logo)
                    .IsRequired()
                    .HasColumnName("LOGO")
                    .HasColumnType("longtext");

                entity.Property(e => e.Morada)
                    .IsRequired()
                    .HasColumnName("MORADA")
                    .HasColumnType("varchar(256)");

                entity.Property(e => e.Nif)
                    .HasColumnName("NIF")
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("NOME")
                    .HasColumnType("varchar(500)");

                entity.Property(e => e.Pagina)
                    .HasColumnName("PAGINA")
                    .HasColumnType("longtext");

                entity.Property(e => e.Phonenumber)
                    .IsRequired()
                    .HasColumnName("PHONENUMBER")
                    .HasColumnType("varchar(20)");

                entity.HasOne(d => d.CriadoPorNavigation)
                    .WithMany(p => p.TdInstituicao)
                    .HasForeignKey(d => d.CriadoPor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TD_INSTITUICAO_TD_USERS");
            });

            modelBuilder.Entity<TdNoticias>(entity =>
            {
                entity.ToTable("TD_NOTICIAS");

                entity.HasIndex(e => e.CriadoPor)
                    .HasName("FK_TD_NOTICIAS_TD_USER_idx");

                entity.HasIndex(e => e.EventoId)
                    .HasName("FK_TD_NOTICIAS_TD_EVENTO_idx");

                entity.HasIndex(e => e.InstId)
                    .HasName("FK_TD_NOTICIAS_TD_INSTITUICAO_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.CriadoPor)
                    .IsRequired()
                    .HasColumnName("CRIADO_POR")
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.DataAlteracao)
                    .HasColumnName("DATA_ALTERACAO")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DATA_CRIACAO")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasColumnName("DESCRICAO")
                    .HasColumnType("varchar(250)");

                entity.Property(e => e.EventoId)
                    .HasColumnName("EVENTO_ID")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.InstId)
                    .IsRequired()
                    .HasColumnName("INST_ID")
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("NOME")
                    .HasColumnType("varchar(250)");

                entity.Property(e => e.Pagina)
                    .IsRequired()
                    .HasColumnName("PAGINA")
                    .HasColumnType("longtext");

                entity.HasOne(d => d.CriadoPorNavigation)
                    .WithMany(p => p.TdNoticias)
                    .HasForeignKey(d => d.CriadoPor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TD_NOTICIAS_TD_USER");

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

                entity.HasIndex(e => e.InstituicaoId)
                    .HasName("FK_TD_TAREFAS_TD_INST_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasColumnName("DESCRICAO")
                    .HasColumnType("longtext");

                entity.Property(e => e.InstituicaoId)
                    .HasColumnName("INSTITUICAO_ID")
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.NumParticMax)
                    .HasColumnName("NUM_PARTIC_MAX")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Turnos)
                    .HasColumnName("TURNOS")
                    .HasColumnType("tinyint(1)");

                entity.HasOne(d => d.Instituicao)
                    .WithMany(p => p.TdTarefas)
                    .HasForeignKey(d => d.InstituicaoId)
                    .HasConstraintName("FK_TD_TAREFAS_TD_INST");
            });

            modelBuilder.Entity<TdTemplates>(entity =>
            {
                entity.ToTable("TD_TEMPLATES");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.DataAlteracao)
                    .HasColumnName("DATA_ALTERACAO")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataCriacao)
                    .HasColumnName("DATA_CRIACAO")
                    .HasColumnType("datetime");

                entity.Property(e => e.Evento)
                    .HasColumnName("EVENTO")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Noticia)
                    .HasColumnName("NOTICIA")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Pagina)
                    .IsRequired()
                    .HasColumnName("PAGINA")
                    .HasColumnType("longtext");
            });

            modelBuilder.Entity<TdTurno>(entity =>
            {
                entity.ToTable("TD_TURNO");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("bigint(20)");

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

                entity.HasIndex(e => e.Id)
                    .HasName("ID")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.ConcurrencyStamp)
                    .IsRequired()
                    .HasColumnName("CONCURRENCY_STAMP")
                    .HasColumnType("longtext");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasColumnType("varchar(256)");

                entity.Property(e => e.NormalizedName)
                    .IsRequired()
                    .HasColumnName("NORMALIZED_NAME")
                    .HasColumnType("varchar(256)");
            });

            modelBuilder.Entity<TdUsers>(entity =>
            {
                entity.ToTable("TD_USERS");

                entity.HasIndex(e => e.Id)
                    .HasName("ID")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.Age)
                    .HasColumnName("AGE")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Bio)
                    .HasColumnName("BIO")
                    .HasColumnType("varchar(500)");

                entity.Property(e => e.ConcurrencyStamp)
                    .IsRequired()
                    .HasColumnName("CONCURRENCY_STAMP")
                    .HasColumnType("longtext");

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
                    .HasColumnType("varchar(256)");

                entity.Property(e => e.EmailConfirmed)
                    .HasColumnName("EMAIL_CONFIRMED")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Genero)
                    .IsRequired()
                    .HasColumnName("GENERO")
                    .HasColumnType("varchar(12)");

                entity.Property(e => e.Imagem)
                    .HasColumnName("IMAGEM")
                    .HasColumnType("longtext");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasColumnType("varchar(256)");

                entity.Property(e => e.NormalizedName)
                    .IsRequired()
                    .HasColumnName("NORMALIZED_NAME")
                    .HasColumnType("varchar(256)");

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasColumnName("PASSWORD_HASH")
                    .HasColumnType("longtext");

                entity.Property(e => e.Phonenumber)
                    .HasColumnName("PHONENUMBER")
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.PhonenumberConfirmed)
                    .HasColumnName("PHONENUMBER_CONFIRMED")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("USERNAME")
                    .HasColumnType("varchar(256)");
            });
        }
    }
}
