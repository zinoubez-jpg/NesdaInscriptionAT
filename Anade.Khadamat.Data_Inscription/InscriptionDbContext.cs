using Anade.Khadamat.Domain;
using Microsoft.EntityFrameworkCore;

namespace Anade.Khadamat.Data_Inscription
{
    public class InscriptionDbContext : DbContext
    {
        public InscriptionDbContext() : base()
        {

        }

        public InscriptionDbContext(DbContextOptions<InscriptionDbContext> options) : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           // optionsBuilder.UseSqlServer(@"Server=webserver\SQL_WEBSITE;Database=INSCRIPTION;Integrated Security=false;User=sa;Password=@BnT02-18+63;");
           // optionsBuilder.UseSqlServer(@"Server=.;Database=KhadamatData;Integrated Security=false;User id=sa;Password=SmartMana_2017_ANSEJ;");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Inscription_Commune>(entity =>
            {

                entity.HasMany(p => p.Inscription_Promoteur)
                    .WithOne(d => d.Inscription_Commune)
                    .HasForeignKey(d => d.CommuneId);

                entity.HasMany(p => p.Inscription_Promoteur1)
                    .WithOne(d => d.Inscription_Commune1)
                    .HasForeignKey(d => d.CommuneHabId);
            });

        modelBuilder.Entity<Inscription_Promoteur>(entity =>
        {
            entity.HasOne(d => d.Inscription_Commune)
                .WithMany(p => p.Inscription_Promoteur)
                .HasForeignKey(d => d.CommuneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Verbinding_BeginStad");

            entity.HasOne(d => d.Inscription_Commune1)
                .WithMany(p => p.Inscription_Promoteur1)
                .HasForeignKey(d => d.CommuneHabId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Verbinding_EindStad");
        });

            //modelBuilder.Entity<Inscription_Commune>().HasOne(commune => commune.Inscription_Promoteur)
            //                           .WithRequired().HasForeignKey(con => con.EndCityId);

            //modelBuilder.Entity<Inscription_Commune>().HasMany(commune => commune.Inscription_Promoteur)
            //                           .WithRequired().HasForeignKey(con => con.StartCityId);

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Inscription_Activite> ACTIVITE { get; set; }
        public virtual DbSet<Inscription_Activite_Tmp> Activité_Tmp { get; set; }
        public virtual DbSet<Inscription_Annexe> ANNEXE { get; set; }
        public virtual DbSet<Inscription_Antenne> ANTENNE { get; set; }
        public virtual DbSet<Inscription_Article> ARTICLE { get; set; }
        public virtual DbSet<Inscription_Associes> ASSOCIES { get; set; }
        public virtual DbSet<Inscription_Calcule_Investissement> CALCULERINVESTISSEMENT { get; set; }
        public virtual DbSet<Inscription_Commune> COMMUNE { get; set; }
        public virtual DbSet<Inscription_Compteur> Compteur { get; set; }
        public virtual DbSet<Inscription_Detail_Dossier> DETAIL_DOSSIER { get; set; }
        public virtual DbSet<Inscription_Dossier_Annuler> Dossier_Annuler { get; set; }
        public virtual DbSet<Inscription_ME_En_Difficulte> ME_En_Difficulte { get; set; }
        public virtual DbSet<Inscription_Motif_Annulation> Motif_Annulation { get; set; }
        public virtual DbSet<Inscription_Niveau> NIVEAU { get; set; }
        public virtual DbSet<Inscription_Notification_Promoteur_En_Difficulte> Notification_Promoteur_En_Difficulte { get; set; }
        public virtual DbSet<Inscription_Pays> PAYS { get; set; }
        public virtual DbSet<Inscription_Phase_Finale> Phase_Finale { get; set; }
        public virtual DbSet<Inscription_PNR_Supp> PNR_SUPP { get; set; }
        public virtual DbSet<Inscription_Promoteur> PROMOTEUR { get; set; }
        public virtual DbSet<Inscription_Qualite> QUALITE { get; set; }
        public virtual DbSet<Inscription_Recapitulatif> Recapitulatif { get; set; }
        public virtual DbSet<Inscription_Rubrique> RUBRIQUE { get; set; }
        public virtual DbSet<Inscription_Secteur_Activite> SECTEUR_ACTIVITE { get; set; }
        public virtual DbSet<Inscription_Sexe> SEXE { get; set; }
        public virtual DbSet<Inscription_Type_Article> Type_Article { get; set; }
        public virtual DbSet<Inscription_Type_Financement> TYPE_FINANCEMENT { get; set; }
        public virtual DbSet<Inscription_Wilaya> WILAYA { get; set; }
        public virtual DbSet<Inscription_Circonscription> CIRCONSCRIPTION { get; set; }
        public virtual DbSet<Inscription_Markers> markers { get; set; }
        public virtual DbSet<Inscription_Categorie> CATEGORIE { get; set; }
        public virtual DbSet<Inscription_Phase> PHASE { get; set; }
        public virtual DbSet<Inscription_Forme_Juridique> Forme_Juridique { get; set; }

    }
}
