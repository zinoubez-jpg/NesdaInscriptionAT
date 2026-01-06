using Anade.Business.Core;
using Anade.Data.Abstractions;
using Anade.Khadamat.Data_Suivi;
using Anade.Khadamat.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Anade.Khadamat.Business
{

    public class PromoteurSuiviBusinesService : GenericBusinessService<Suivi_Promoteur, int>
    {
        public PromoteurSuiviBusinesService(IUnitOfWork<SuiviDbContext> unitOfWork) : base(unitOfWork)
        {

        }

        public List<Suivi_Promoteur> GetByInformationsPromoteur(Inscription_Promoteur promoteur)
        {
            var Num_Acte_Int = int.Parse(promoteur.NUM_ACT_P);
            var Num_Acte_Avec_Zéro = promoteur.NUM_ACT_P;
            var Num_Acte_Sans_Zéro = Num_Acte_Int.ToString();
            if (promoteur.NUM_ACT_P.Length < 6)
            {
                var n = 6 - promoteur.NUM_ACT_P.Length;
                for (int i = 0; i < n; i++)
                {
                    Num_Acte_Avec_Zéro = "0" + Num_Acte_Avec_Zéro;
                }
            }

            List<Suivi_Promoteur> result = GetAllFiltered(x => x.NOM_P.ToUpper().Trim() == promoteur.NOM_P.ToUpper().Trim()
                                                  && x.PRENOM_P.ToUpper().Trim() == promoteur.PRENOM_P.ToUpper().Trim()
                                                  && x.DDN_P == promoteur.DDN_P
                                                  && x.COMMUNE_NAIS == promoteur.Inscription_Commune.Code
                                                  && (x.NUM_ACT_P == Num_Acte_Avec_Zéro || x.NUM_ACT_P == Num_Acte_Sans_Zéro)
                                                  );
            return result;
        }

        public List<Suivi_Promoteur> GetByInformationsPromoteur2(string NOM_P, string PRENOM_P, DateTime DDN_P, string NUM_ACT_P, string COMMUNE_NAIS)
        {
            var Num_Acte_Int = int.Parse(NUM_ACT_P);
            var Num_Acte_Avec_Zéro = NUM_ACT_P;
            var Num_Acte_Sans_Zéro = Num_Acte_Int.ToString();
            if (NUM_ACT_P.Length < 6)
            {
                var n = 6 - NUM_ACT_P.Length;
                for (int i = 0; i < n; i++)
                {
                    Num_Acte_Avec_Zéro = "0" + Num_Acte_Avec_Zéro;
                }
            }

            List<Suivi_Promoteur> result = GetAllFiltered(x => x.NOM_P.ToUpper().Trim() == NOM_P.ToUpper().Trim()
                                                  && x.PRENOM_P.ToUpper().Trim() == PRENOM_P.ToUpper().Trim()
                                                  && x.DDN_P == DDN_P
                                                  && x.COMMUNE_NAIS == COMMUNE_NAIS
                                                  && (x.NUM_ACT_P == Num_Acte_Avec_Zéro || x.NUM_ACT_P == Num_Acte_Sans_Zéro)
                                                  );
            return result;
        }

        public List<Suivi_Promoteur> GetAllFilteredByCodePromoteurs(List<Inscription_Promoteur> promoteurs, bool value)
        {
            if (promoteurs == null || !promoteurs.Any())
                return new List<Suivi_Promoteur>();

            // Extraire les codes en mémoire
            var codesPromoteurs = promoteurs.Select(p => p.CODE_P).ToList();

            // Utiliser Contains qui peut être traduit en SQL IN clause
            return GetAllFiltered(x => codesPromoteurs.Contains(x.CODE_P) && x.AVOIR_EXTENSION == value);
        }

        public List<Suivi_Promoteur> GetAllFilteredByInfosPromoteur(Inscription_Promoteur promoteur, bool value)
        {

            var Num_Acte_Int = int.Parse(promoteur.NUM_ACT_P);
            var Num_Acte_Avec_Zéro = promoteur.NUM_ACT_P;
            var Num_Acte_Sans_Zéro = Num_Acte_Int.ToString();
            
            if (promoteur.NUM_ACT_P.Length < 6)
            {
                var n = 6 - promoteur.NUM_ACT_P.Length;
                for (int i = 0; i < n; i++)
                {
                    Num_Acte_Avec_Zéro = "0" + Num_Acte_Avec_Zéro;
                }
            }

            return GetAllFiltered(x => promoteur.NOM_P.ToUpper().Trim() == x.NOM_P.ToUpper().Trim()
                                    && promoteur.PRENOM_P.ToUpper().Trim() == x.PRENOM_P.ToUpper().Trim()
                                    && promoteur.DDN_P == x.DDN_P
                                    && promoteur.Inscription_Commune.Code == x.COMMUNE_NAIS
                                    && (promoteur.NUM_ACT_P == Num_Acte_Avec_Zéro || promoteur.NUM_ACT_P == Num_Acte_Sans_Zéro)
                                    && x.AVOIR_EXTENSION == value
                                 );
        }

        public Suivi_Promoteur GetByCodeNumDoss(string code, string numdoss)
        {
            Suivi_Promoteur promoteur = new();

            promoteur = GetSingle(x => x.CODE_P == code || x.NUM_DOSS == numdoss);

            return promoteur;
        }
    }
}
