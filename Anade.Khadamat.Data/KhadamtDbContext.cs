using Anade.Khadamat.Domain;
using Microsoft.EntityFrameworkCore;

namespace Anade.Khadamat.Data
{
    public class KhadamtDbContext : DbContext
    {
        public KhadamtDbContext() : base()
        {
        }
        public KhadamtDbContext(DbContextOptions<KhadamtDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Inscription>().HasOne(x => x.CommuneNaissance)
                          .WithMany(x => x.CommuneNaissances)
                          .HasForeignKey(x => x.CommuneNaissanceId)
                          .OnDelete(DeleteBehavior.NoAction);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Inscription>().HasOne(x => x.CommuneResidence)
               .WithMany(x => x.CommuneResidences)
               .HasForeignKey(x => x.CommuneResidenceId)
               .OnDelete(DeleteBehavior.NoAction);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Inscription>().HasOne(x => x.CommuneSiegeSocial)
              .WithMany(x => x.CommuneSeigeSocials)
              .HasForeignKey(x => x.CommuneSeigeSocialId)
              .OnDelete(DeleteBehavior.NoAction);
            base.OnModelCreating(modelBuilder);




        }
        public virtual DbSet<Inscription> Inscriptions { get; set; }
        public virtual DbSet<Commune> Communes { get; set; }
        public virtual DbSet<Wilaya> Wilayas { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<SituationFamiliale> SituationFamiliales { get; set; }
        public virtual DbSet<TypeInscription> TypesInscription { get; set; }
        public virtual DbSet<CSVF> CSVFs { get; set; }
        public virtual DbSet<Decision> Decisions { get; set; }
        public virtual DbSet<EvaluationProjet> EvaluationProjets { get; set; }
        public virtual DbSet<SessionFormation> SessionFormations { get; set; }
        public virtual DbSet<TypeDecision> TypeDecisions { get; set; }
        public virtual DbSet<Formation> Formations { get; set; }




    }
}
