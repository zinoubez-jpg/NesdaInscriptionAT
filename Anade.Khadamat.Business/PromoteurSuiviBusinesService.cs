using Anade.Business.Core;
using Anade.Data.Abstractions;
using Anade.Khadamat.Data_Suivi;
using Anade.Khadamat.Domain;
using System;
using System.Collections.Generic;
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

        public Suivi_Promoteur GetByCodeNumDoss(string code, string numdoss)
        {
            Suivi_Promoteur promoteur = new();

            promoteur = GetSingle(x => x.CODE_P == code || x.NUM_DOSS == numdoss);

            return promoteur;
        }
    }
}
