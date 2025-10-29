using Anade.Khadamat.Domain;
using Microsoft.EntityFrameworkCore;

namespace Anade.Khadamat.Data_Authentification
{
    public class AuthentificationDbContext : DbContext
    {
        public AuthentificationDbContext() : base()
        {

        }

        public AuthentificationDbContext(DbContextOptions<AuthentificationDbContext> options) : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server=webserver\SQL_WEBSITE;Database=AUTHENTIFICATION;Integrated Security=false;User=sa;Password=@BnT02-18+63;");
            // optionsBuilder.UseSqlServer(@"Server=.;Database=KhadamatData;Integrated Security=false;User id=sa;Password=SmartMana_2017_ANSEJ;");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {           
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Authentification_Activation_Compte> Activation_Compte { get; set; }
        public virtual DbSet<Authentification_Compte> Compte { get; set; }
        public virtual DbSet<Authentification_Etat> Etat { get; set; }
        public virtual DbSet<Authentification_Identifiant_SI> Identifiant_SI { get; set; }
        public virtual DbSet<Authentification_Notification> Notification { get; set; }
        public virtual DbSet<Authentification_Passe_Oublier> Passe_Oublier { get; set; }
        public virtual DbSet<Authentification_Type_Compte> Type_Compte { get; set; }
        public virtual DbSet<Authentification_Visite> Visite { get; set; }
        public virtual DbSet<Authentification_CDE> CDE { get; set; }
        public virtual DbSet<Authentification_Compte_CDE> Compte_CDE { get; set; }
        public virtual DbSet<Authentification_Erreur_CDE_Compte> Erreur_CDE_Compte { get; set; }
    }
}
