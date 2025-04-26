using Anade.Business.Core;
using Anade.Data.Abstractions;
using Anade.Khadamat.Domain;
using Microsoft.Extensions.Options;
using System;
using System.Linq.Expressions;

namespace Anade.Khadamat.Business
{
    public class InscriptionBusinessService : GenericBusinessService<Inscription, int>
    {
        private readonly IOptions<PromoteurEligibiliteSettings> _promoteurEligibiliteSettings;


        public InscriptionBusinessService(IUnitOfWork unitOfWork, IOptions<PromoteurEligibiliteSettings> promoteurEligibiliteSettings) : base(unitOfWork)
        {
            _promoteurEligibiliteSettings = promoteurEligibiliteSettings;

        }


        protected override void OnAdding(Inscription entity)
        {
            double age = DateTime.Now.Subtract(entity.DateNaissance).TotalDays / 365;

            if (_repository.Count(x =>
                                  x.Validee == true &&
                                  x.NIN == entity.NIN) > 0)
                throw new BusinessException("هناك شخص آخر مسجل بنفس رقم التعريف الوطني.");

            if (age < _promoteurEligibiliteSettings.Value.MinAge || age > _promoteurEligibiliteSettings.Value.MaxAge)
                throw new BusinessException($"لتسجيل، يجب أن يكون عمرك بين 20و 58سنة");
            base.OnAdding(entity);

        }

        protected override void OnUpdating(Inscription entity)
        {
            double age = DateTime.Now.Subtract(entity.DateNaissance).TotalDays / 365;


            if (_repository.Count(x =>
                                  x.Id != entity.Id &&
                                  x.NIN == entity.NIN) > 0)
                throw new BusinessException("Une personne avec le même NIN existe déjà.");

            if (age < _promoteurEligibiliteSettings.Value.MinAge || age > _promoteurEligibiliteSettings.Value.MaxAge)
                throw new BusinessException($"La condition d'age n'est pas respectée");

            base.OnUpdating(entity);
        }




        public override Expression<Func<Inscription, object>>[] GetDefaultLoadProperties()
        {
            Expression<Func<Inscription, object>> loadCommuneRes = x => x.CommuneResidence;
            Expression<Func<Inscription, object>> loadCommuneNais = x => x.CommuneNaissance;
            Expression<Func<Inscription, object>> loadCommuneSS = x => x.CommuneSiegeSocial;
            Expression<Func<Inscription, object>> loadGenre = x => x.Genre;
            Expression<Func<Inscription, object>> loadSituationFamiliale = x => x.SituationFamiliale;
            Expression<Func<Inscription, object>> loadTypeInscription = x => x.TypeInscription;
            Expression<Func<Inscription, object>> loadSessionFormations = x => x.SessionFormations;
            Expression<Func<Inscription, object>> loadFiches = x => x.EvaluationProjets;
            Expression<Func<Inscription, object>> loadCSVF = x => x.CSVFs;
            Expression<Func<Inscription, object>> loadTypeFinancement = x => x.TypeFinancement;
            Expression<Func<Inscription, object>> loadActivite = x => x.Activite;
            Expression<Func<Inscription, object>> loadCategorie = x => x.Categorie;


            return new Expression<Func<Inscription, object>>[] { loadCommuneRes, loadCommuneNais, loadCommuneSS, loadGenre, loadSituationFamiliale, loadTypeInscription , loadSessionFormations, loadFiches , loadCSVF, loadTypeFinancement , loadActivite, loadCategorie };
        }
    }
}

