using Anade.Khadamat.Domain;
using Microsoft.EntityFrameworkCore;

namespace Anade.Khadamat.Data_Suivi
{
    public class SuiviDbContext : DbContext
    {
        public SuiviDbContext() : base()
        {

        }

        public SuiviDbContext(DbContextOptions<SuiviDbContext> options) : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           // optionsBuilder.UseSqlServer(@"Server=webserver\SQL_WEBSITE;Database=Suivi;Integrated Security=false;User=sa;Password=@BnT02-18+63;");
            base.OnConfiguring(optionsBuilder);
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{           
        //    base.OnModelCreating(modelBuilder);
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Suivi_Contentieux>()
                .HasKey(e => new { e.Code_Cas_Contentieux, e.Num_Doss, e.Code_Nat_Projet, e.Date_debut });
            modelBuilder.Entity<Suivi_Version>().HasNoKey();
            // Configuration de la relation Promoteur -> Evenements
            modelBuilder.Entity<Suivi_Promoteur>()
                .HasMany(p => p.Evenement)
                .WithOne(e => e.PROMOTEUR)
                .HasForeignKey(e => e.Code_P)
                .HasPrincipalKey(p => p.CODE_P)
                .OnDelete(DeleteBehavior.Cascade); // ou Restrict selon vos besoins

            // Configuration de la clé primaire pour Suivi_Evenement
            modelBuilder.Entity<Suivi_Evenement>()
                .HasKey(e => new { e.Code_P, e.Id }); // Correspond à votre PK en base [Code_P, id]
            modelBuilder.Entity<Suivi_RemboursementPNR>().HasNoKey();

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Suivi_Annexe> ANNEXE { get; set; }
        public virtual DbSet<Suivi_Antenne> ANTENNE { get; set; }
        public virtual DbSet<Suivi_CasContentieux> CAS_CONTENTIEUX { get; set; }
        public virtual DbSet<Suivi_Contentieux> CONTENTIEUX { get; set; }
        public virtual DbSet<Suivi_Documents> Documents { get; set; }
        public virtual DbSet<Suivi_Evenement> Evenement { get; set; }
        public virtual DbSet<Suivi_MiseAJour> MAJ { get; set; }
        public virtual DbSet<Suivi_Promoteur> PROMOTEUR { get; set; }
        public virtual DbSet<Suivi_PromoteurDesiste> PROMOTEUR_Desiste { get; set; }
        public virtual DbSet<Suivi_PromoteurInscrits> PROMOTEUR_inscrits { get; set; }
        public virtual DbSet<Suivi_PromoteurNonActifs> PromoteursNonActifs { get; set; }
        public virtual DbSet<Suivi_RemboursementPNR> REMB_PNR { get; set; }
        public virtual DbSet<Suivi_Version> Version { get; set; }

    }
}
