using Anade.Business.Core;
using Anade.Data.Abstractions;
using Anade.Khadamat.Data_Inscription;
using Anade.Khadamat.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Anade.Domain.Core.ValueTypeExtensions;
using System.Linq;

namespace Anade.Khadamat.Business
{
    public class AssociesInscriptionBusinesService : GenericBusinessService<Inscription_Associes, int>
    {
        protected readonly CompteAuthentificationBusinesService _compteAuthentificationBusinesService;
        protected readonly Inscription_CirconscriptionBusinessService _inscription_CirconscriptionBusinessService;
        protected readonly Inscription_CommuneBusinesService _inscription_CommuneBusinesService;
        protected readonly Inscription_AnnexeBusinesService _inscription_AnnexeBusinesService;
        protected readonly Inscription_AntenneBusinesService _inscription_AntenneBusinesService;
        protected readonly PromoteurSuiviBusinesService _promoteurSuiviBusinesService;
        protected readonly Authentification_CDEBusinesService _authentificationCDEBusinesService;
        protected readonly Authentification_Compte_CDEBusinesService _authentification_Compt_CDEBusinesService;
        protected readonly CentreDeveloppementEntrepreneuriatBusinessService _centreDeveloppementEntrepreneuriatBusinessService;
        protected readonly Detail_DossierInscriptionBusinesService _detail_DossierInscriptionBusinessService;
        protected readonly Identifiant_SIAuthentificationBusinesService _identifiant_SIAuthentificationBusinesService;
        protected readonly PromoteurInscriptionBusinesService _promoteurInscriptionBusinessService;

        public AssociesInscriptionBusinesService(IUnitOfWork<InscriptionDbContext> unitOfWork
            , CompteAuthentificationBusinesService compteAuthentificationBusinesService
            , Inscription_CirconscriptionBusinessService inscription_CirconscriptionBusinessService
            , Inscription_CommuneBusinesService inscription_CommuneBusinesService
            , Inscription_AnnexeBusinesService inscription_AnnexeBusinesService
            , Inscription_AntenneBusinesService inscription_AntenneBusinesService
            , PromoteurSuiviBusinesService promoteurSuiviBusinesService
            , Authentification_CDEBusinesService authentificationCDEBusinesService
            , Authentification_Compte_CDEBusinesService authentification_Compt_CDEBusinesService
            , CentreDeveloppementEntrepreneuriatBusinessService centreDeveloppementEntrepreneuriatBusinessService
            , Detail_DossierInscriptionBusinesService detail_DossierInscriptionBusinessService
            , Identifiant_SIAuthentificationBusinesService identifiant_SIAuthentificationBusinesService
            , PromoteurInscriptionBusinesService promoteurInscriptionBusinessService
            ) : base(unitOfWork)
        {
            _compteAuthentificationBusinesService = compteAuthentificationBusinesService;
            _inscription_CirconscriptionBusinessService = inscription_CirconscriptionBusinessService;
            _inscription_CommuneBusinesService = inscription_CommuneBusinesService;
            _inscription_AnnexeBusinesService = inscription_AnnexeBusinesService;
            _inscription_AntenneBusinesService = inscription_AntenneBusinesService;
            _promoteurSuiviBusinesService = promoteurSuiviBusinesService;
            _authentificationCDEBusinesService = authentificationCDEBusinesService;
            _authentification_Compt_CDEBusinesService = authentification_Compt_CDEBusinesService;
            _centreDeveloppementEntrepreneuriatBusinessService = centreDeveloppementEntrepreneuriatBusinessService;
            _detail_DossierInscriptionBusinessService = detail_DossierInscriptionBusinessService;
            _identifiant_SIAuthentificationBusinesService = identifiant_SIAuthentificationBusinesService;
            _promoteurInscriptionBusinessService = promoteurInscriptionBusinessService;
        }

        public override Expression<Func<Inscription_Associes, object>>[] GetDefaultLoadProperties()
        {
            Expression<Func<Inscription_Associes, object>> loadCategorie = x => x.Inscription_Categorie;

            return new Expression<Func<Inscription_Associes, object>>[] { loadCategorie };
        }

        protected override void OnAdding(Inscription_Associes entity)
        {
            //Création compte dans la base [AUTHENTIFICATION]
            _ = entity.Inscription_Promoteur.NUM_ACT_P.Trim().PadLeft(6, '0');
            var Num_Acte_Int = int.Parse(entity.Inscription_Promoteur.NUM_ACT_P);

            var Num_Acte_Avec_Zéro = entity.Inscription_Promoteur.NUM_ACT_P;
            var Num_Acte_Sans_Zéro = Num_Acte_Int.ToString();
            if (entity.Inscription_Promoteur.NUM_ACT_P.Length < 6)
            {
                var n = 6 - entity.Inscription_Promoteur.NUM_ACT_P.Length;
                for (int i = 0; i < n; i++)
                {
                    Num_Acte_Avec_Zéro = "0" + Num_Acte_Avec_Zéro;
                }
            }

            var commune_ele = _inscription_CommuneBusinesService.GetById(entity.Inscription_Promoteur.CommuneId);
            var code_promoteur = commune_ele.Code + entity.Inscription_Promoteur.DDN_P.Year + Num_Acte_Avec_Zéro;

            entity.Inscription_Promoteur.Inscription_Commune = commune_ele;

            //Si le promoteur gérant existe déja dans sieje Base de données Suivi
            var promoteuractif = _promoteurSuiviBusinesService.GetByInformationsPromoteur(entity.Inscription_Promoteur);

            if (promoteuractif.Count != 0)
            {
                throw new BusinessException("Vous possèdez déjà un dossier à NESDA.Veuillez bien vérifier votre date,commune  et  numéro d'acte de naissance.");
            }
            else
            {
                //Si le promoteur gérant existe déja dans inscription en ligne
                var promoteurExisteInscription = GetSingle(x => x.Inscription_Promoteur.DDN_P == entity.Inscription_Promoteur.DDN_P && x.Inscription_Promoteur.CommuneId == entity.Inscription_Promoteur.CommuneId && (x.Inscription_Promoteur.NUM_ACT_P == Num_Acte_Avec_Zéro || x.Inscription_Promoteur.NUM_ACT_P == Num_Acte_Sans_Zéro));

                if (promoteurExisteInscription != null)
                {
                    throw new BusinessException("Vous étes déjà inscrit dans notre espace.");
                }
                else
                {
                    var promoteurExisteNN = GetSingle(x => x.Inscription_Promoteur.NIN == entity.Inscription_Promoteur.NIN);

                    if (promoteurExisteNN != null)
                    {
                        throw new BusinessException("NINExist");
                    }
                    else
                    {
                        var ListCode_P = GetAllFiltered(o => o.Inscription_Promoteur.DDN_P.Year == entity.Inscription_Promoteur.DDN_P.Year && o.Inscription_Promoteur.CommuneId == entity.Inscription_Promoteur.CommuneId && (o.Inscription_Promoteur.NUM_ACT_P == Num_Acte_Avec_Zéro || o.Inscription_Promoteur.NUM_ACT_P == Num_Acte_Sans_Zéro));

                        if (ListCode_P.Count == 0)
                            code_promoteur += "0";
                        else
                        {
                            List<int> sequences = new List<int>();
                            foreach (var item in ListCode_P)
                            {
                                sequences.Add(int.Parse(item.Inscription_Promoteur.CODE_P.Last().ToString()));
                            }
                            code_promoteur = code_promoteur + "" + (sequences.Max() + 1);
                        }
                    }
                }
            }

            entity.Inscription_Promoteur.CODE_P = code_promoteur;

            base.OnAdding(entity);
        }

        public override BusinessResult Add(Inscription_Associes entity)
        {
            BusinessResult businessResult;
            try
            {
                OnAdding(entity);
                _repository.Add(entity);

                if (_unitOfWork.SaveChanges() == 0)
                {
                    throw new DataNotUpdatedException("Opération non enregsitrée niveau Promoteur!");
                }

                OnAdded(entity);

                businessResult = BusinessResult.Success;
            }
            catch (DataNotUpdatedException ex)
            {
                businessResult = new BusinessResult(false);
                businessResult.Messages.Add(new MessageResult { Message = ex.Message, MessageType = MessageType.Warning });
            }
            catch (BusinessException ex)
            {
                businessResult = new BusinessResult(false);
                businessResult.Messages.Add(new MessageResult { Message = ex.Message, MessageType = MessageType.Warning });
            }
            catch (Exception ex)
            {
                businessResult = new BusinessResult(false);
                businessResult.Messages.Add(new MessageResult { Message = ex.Message, MessageType = MessageType.Warning });
            }

            return businessResult;
        }

        private Authentification_Compte Create_Compte(Inscription_Associes entityAssocies, CentreDeveloppementEntrepreneuriat cdeEle)
        {
            Inscription_Promoteur entity = entityAssocies.Inscription_Promoteur;

            Authentification_Compte entityCompte = new();
            Authentification_Identifiant_SI entityIdentifiantSI = new();
            Authentification_Compte_CDE entityCompteCDE = new();
            Authentification_CDE entityCDE = _authentificationCDEBusinesService.GetByCodeNew(cdeEle.Code);

            // Affectation d'une annexe au compte se basant sur son commune habitation.
            Inscription_Circonscription inscription_Circonscription;

            if (entity.Id_Commune_Siege_Social != null && entity.Id_Commune_Siege_Social != 1)
            {
                inscription_Circonscription = _inscription_CirconscriptionBusinessService.GetByCommuneId((int)entity.Id_Commune_Siege_Social);
                entityCompte.Code_Annexe = inscription_Circonscription.Inscription_Annexe.Code;
                entity.Code_Annexe = inscription_Circonscription.Inscription_Annexe.Code;
            }
            else
            {
                if (entity.Id_Annexe_Etranger == null)
                {
                    inscription_Circonscription = _inscription_CirconscriptionBusinessService.GetByCommuneId(entity.CommuneHabId);
                    entityCompte.Code_Annexe = inscription_Circonscription.Inscription_Annexe.Code;
                    entity.Code_Annexe = inscription_Circonscription.Inscription_Annexe.Code;
                }
                else
                {
                    var AgenceWilaya = _inscription_AntenneBusinesService.GetById((int)entity.Id_Annexe_Etranger);

                    string sieg = AgenceWilaya.Code.Substring(0, 2);
                    string sieg_res = sieg + "01";

                    if (AgenceWilaya.Code == "1601")
                    {
                        sieg_res = "1602";
                    }
                    else if (AgenceWilaya.Code == "1602")
                    {
                        sieg_res = "1614";
                    }
                    else if (AgenceWilaya.Code == "1603")
                    {
                        sieg_res = "1611";
                    }
                    else if (AgenceWilaya.Code == "1604")
                    {
                        sieg_res = "1603";
                    }

                    var AnnexeChefLieu = _inscription_AnnexeBusinesService.GetByCodeAnnexe(sieg_res);
                    entityCompte.Code_Annexe = AnnexeChefLieu.Code;
                    entity.Code_Annexe = AnnexeChefLieu.Code;
                }
            }

            entityCompte.Date_Creation = DateTime.Now;
            entityCompte.Pseudo = entity.EMAIL_P;
            entityCompte.Authentification_Type_CompteId = 1;

            if (entity.Ex_Detenu)
            {
                entityCompte.Authentification_EtatId = 1;
                entityCompte.Mot_De_Passe = entity.Mot_Passe_RandomPassword_9_ToSHA;
            }
            else
            {
                entityCompte.Authentification_EtatId = 0;
                entityCompte.Mot_De_Passe = entity.Mot_Passe_ToSHA;
            }

            entityCompte.Si_Inscription = true;

            //entityCompteCDE.Authentification_CompteId = entityCompte.Id;
            entityCompteCDE.Authentification_CDEId = entityCDE.Id;
            entityCompteCDE.Authentification_CDE = entityCDE;
            entityCompteCDE.Authentification_Compte = entityCompte;

            Inscription_Detail_Dossier detail_Dossier = _detail_DossierInscriptionBusinessService.GetById(entityAssocies.Inscription_Detail_DossierId);

            //entityIdentifiantSI.Authentification_CompteId = entityCompte.Id;
            entityIdentifiantSI.Num_SI = detail_Dossier.Id.ToString();
            entityIdentifiantSI.Code_P = entity.CODE_P;
            entityIdentifiantSI.Authentification_Compte = entityCompte;

            List<Authentification_Identifiant_SI> listIdentifiantSI = new();
            List<Authentification_Compte_CDE> listCompteCDE = new();

            listIdentifiantSI.Add(entityIdentifiantSI);
            listCompteCDE.Add(entityCompteCDE);

            entityCompte.Authentification_Identifiant_SI = listIdentifiantSI;
            entityCompte.Authentification_Compte_CDE = listCompteCDE;

            return entityCompte;
        }

        public virtual BusinessResult Add_Compte(Authentification_Compte entity)
        {
            BusinessResult businessResult = _compteAuthentificationBusinesService.Add(entity);

            return businessResult;
        }

        public BusinessResult AddInscription(Inscription_Associes entity, CentreDeveloppementEntrepreneuriat cdeEle)
        {
            BusinessResult businessResult;

            if (Add(entity).Succeeded)
            {
                Authentification_Compte entityCompte = Create_Compte(entity, cdeEle);

                if (!Add_Compte(entityCompte).Succeeded)
                {
                    DeleteAssocie(entity);
                    businessResult = BusinessResult.Failure;
                }
                else
                {
                    businessResult = BusinessResult.Success;
                }
            }
            else
            {
                businessResult = BusinessResult.Failure;
            }

            return businessResult;
        }

        protected void DeleteAssocie(Inscription_Associes entity)
        {
            Inscription_Promoteur promoteur = _promoteurInscriptionBusinessService.GetById(entity.Inscription_PromoteurId);
            Inscription_Detail_Dossier detail_Dossier = _detail_DossierInscriptionBusinessService.GetById(entity.Inscription_Detail_DossierId);

            Delete(entity);
            _promoteurInscriptionBusinessService.Delete(promoteur);
            _detail_DossierInscriptionBusinessService.Delete(detail_Dossier);
        }

    }
}
