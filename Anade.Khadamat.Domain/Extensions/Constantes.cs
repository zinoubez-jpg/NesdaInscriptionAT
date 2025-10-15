using System.Collections.Generic;

namespace Anade.Khadamat.Domain
{
    public static class Ids
    {

        public const int nonDefiniId = 1;
        //
        public const int nbrMinimumAssocie = 1;
        public const int RequiredChoiceGerant = 2;



        //Genre
        public static int Femme = 1;
        public static int Homme = 2;

        //Qualite
        public static int Gerant = 3;
        public static int Associe = 2;     

        //Etat
        public static int AvantValidation = 2;
        public static int EnCoursDeValidation = 3;
        public static int Valide = 4;
        public static int Annule = 5;

        //pays
        public const int Algeria = 4;
        public const int CommuneEtranger = 1543;
        public const int wilayaEtranger = 71;

        //type zone 
        public const int zoneNormale = 2; 

        //TypeFinancement
        public static int AutoFinancement = 2;
        public static int Mixte = 3;
        public static int Triangulaire = 4;

        public const int Alger = 17;

        //niveau instruction 
        public const int Universitaire = 5;
        
        //sourceDiplome
        public const int Universite = 2;

        //Notification
        public const int ValidationNotification = 5;
        public const int ConfirmationNotification = 2;
        public const int PremiereNotification = 3;
        public const int DeuxiemeNotification = 4;

        //ObjectNotification
        public const int Invitation = 2;
        public const int InvitationEntretienIndividuel = 3;
        public const int InvitationCSVF = 4;
        public const int InvitationCompleterDossier = 5;
        public const int InvitationRetraitDocuement = 6;

        //TypePretNonRemunere
        public const int classique = 2;
        public const int cg = 3;
        public const int LO = 4;
        public const int VA = 5;
        public const int Exploitation = 6;

        //Categorie 
        public const int Chomeur = 2;
        public const int Etudiant = 3;
        public const int Fonctionnaire = 4;
        public const int Artisan = 5;
        public const int Fellah = 6;
        public const int RegistreCommerce = 7;
        public const int ProfessionLiberale =8;
        public const int CNAC =9;

        //categorieResultante
        public const int CategoriesChomeurEtudiant = 2;
        public const int CategoriesCNAC = 4;
        public const int CategoriesActif = 3;

        //Phase
        public const int Creation = 2;
        public  const int Extention = 3;

        //SituationFamiliale 
        public const int Married = 3;
        public const int Single = 2;

        //TypeZone
        public const int ZoneNormale = 2;
        public const int HautsPlateaux = 3;
        public const int Sud = 4;
        public const int ZoneSpecifique = 5;
        public const int HautsPlateauxSpecifique = 6;
        public const int SudSpecifique = 7;


        //NatureFacturev
        public const int Equipement = 2;
        public const int Amenagement = 3;
        public const int Assurance = 4;

        //TypeFacture
        public const int PROFORMA = 2;
        public const int DEFINITIVE = 3;
        //resultat Session Validation
        public const int Aucun = 0;
        public const int Accepte = 1;
        public const int Rejete = 2;

        //Type Suppression         
        public const int Annulation = 1;
	    public const int Desistement = 2;

        //motif suppression 
        public const int FalsificationDocuments = 17;

        //type Action
        public const int Consultation = 0;
	    public const int SetQuality = 3;
	    public const int Modification = 1;
      

        //Type structure  
        public const int DG = 2;
        public const int AgenceWilaya = 3;
        public const int Antenne = 4;
       
        //Regime Fiscal 
        public const int Forfaitaire = 2;
        public const int Reel = 3;

        //typeGroupeAD
       
         public const int NIVEAU = 2;
         public const int STRUCTURE = 3;
         public const int ROLE = 4;


        //GroupeAD
          public const int SIG_DG = 2;
          public const int SIG_AGENCES = 3;
          public const int SIG_ANTENNES = 4;
        public const string SIG_ = "SIG_";
        public const string SIG_AN_ = "SIG_AN_";
        public const string SIG_AW_ = "SIG_AW_";
    }

    public static class Codes
    {
        // pour genereation du fichier .fic
        
        //Qualite
        public static string Gerant = "2";
        public static string Associe = "1";    

        //pour verifier l'unicité dans sieje 
        public const string CodecommuneSieje = "9999";
        public const string Code4Zero = "0000";
       
        //Niveau 
        public const int DG = 1;
        public const int AgenceWilaya = 2;
        public const int Antenne = 3;


    }
   
    public static class duree
    {
        //apres 45 jours  & 2 notification & motif ==> promoteur ne s'est pas présenté l'inscription peut etre annuler par le chef d'annexe 
        public const int annulation = 45;

    }
   

    public static class FiltreNonValides
    {
        //Inscriptions non validés
        public static string UnnotifiedAfter24h = "PromoteursUnnotifiedAfter24h";
        public static string UnnotifiedBtw1_5days = "PromoteursUnnotifiedBtw1_5days";
        public static string UnnotifiedAfter5days = "PromoteursUnnotifiedAfter5days";
        public static string NotifiedAfter5days = "PromoteursNotifiedAfter5days";
        public static string InvalidesAfter5daysNotif = "PromoteurInvalidesAfter5daysNotif";

    }
    public static class FiltreValides
    {
        //Inscriptions validés
        public static string withNumSieje = "PromoteursValideswithNumSieje";
        public static string withoutNumSieje = "PromoteursValideswithNumSieje";
      
    }
    public static class Keys
    {
        public const string userSettings = "userSettings";
    }
    public static class swal
    {
        public const string success = "success";
        public const string warning = "warning";
        public const string error = "error";

    }
    public static class AuthorizationPolicyNames //la table Sec.Fonctionnalite dans sigemeDataBase
    {
        //Features PolicyNames
        public static string ReferentielFeature => "Referentiel";
        public static string GelActiviteFeature => "GelActivite";

        public static string EditProjetFeature => "EditerProjet";

        public static string EditFournisseurFeature => "EditFournisseur";

        public static string GelFournisseurFeature => "GelFournisseur";

        public static string DegelFournisseurFeature => "DegelFournisseur";
        public static string ControlGelFournisseurFeature => "ControlGelFournisseur";

        public static string SuppressionFournisseurFeature => "SuppressionFournisseur";

    }

}

