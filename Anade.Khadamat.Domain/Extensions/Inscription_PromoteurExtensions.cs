using Anade.Domain.Core.ValueTypeExtensions;
using System;
using System.Linq.Expressions;

namespace Anade.Khadamat.Domain
{
    public static class Inscription_PromoteurExtensions
    {
        public static bool IsHomme(this Inscription_Promoteur promoteur) => promoteur.Inscription_SexeId == Ids.Homme;
        public static bool IsFemme(this Inscription_Promoteur promoteur) => promoteur.Inscription_SexeId == Ids.Femme;
        public static bool IsResidentEtrange(this Inscription_Promoteur promoteur) => promoteur.Inscription_PaysId != Ids.Algeria;
        public static bool IsNaissanceEtrange(this Inscription_Promoteur promoteur) => promoteur.CommuneId== Ids.CommuneEtranger;
      
        #region Conditions on promoteur
        private static bool OneDigitMonth(Inscription_Promoteur promoteur)
         => promoteur.DDN_P.Month < 10;
        
        private static bool OneDigitDay(Inscription_Promoteur promoteur)
        =>  promoteur.DDN_P.Day < 10;
        
        private static Expression<Func<Inscription_Promoteur, bool>> CodeEtrangeExiste(Inscription_Promoteur promoteur)
       
            => (x => x.DDN_P == promoteur.DDN_P
                      && x.CommuneId == Ids.CommuneEtranger
                      && x.NUM_ACT_P == promoteur.NUM_ACT_P
                      && x.Inscription_Sexe == promoteur.Inscription_Sexe );

        private static Expression<Func<Inscription_Promoteur, bool>> CodeLocalExiste(Inscription_Promoteur promoteur)
       =>  (x => x.DDN_P== promoteur.DDN_P
                      && x.CommuneId == promoteur.CommuneId
                        && x.NUM_ACT_P == promoteur.NUM_ACT_P
                      && x.Inscription_Sexe == promoteur.Inscription_Sexe);
        
        #endregion

    }
}
