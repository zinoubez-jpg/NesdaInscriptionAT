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
    public class PromoteurInscriptionBusinesService : GenericBusinessService<Inscription_Promoteur, int>
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
        // protected readonly AssociesInscriptionBusinesService _associesInscriptionBusinessService;
        protected readonly Identifiant_SIAuthentificationBusinesService _identifiant_SIAuthentificationBusinesService;
        protected readonly Authentification_Erreur_CDE_CompteBusinesService _authentification_Erreur_CDE_CompteBusinesService;



        public PromoteurInscriptionBusinesService(
            IUnitOfWork<InscriptionDbContext> unitOfWork
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
            // , AssociesInscriptionBusinesService associesInscriptionBusinessService
            , Identifiant_SIAuthentificationBusinesService identifiant_SIAuthentificationBusinesService
            , Authentification_Erreur_CDE_CompteBusinesService authentification_Erreur_CDE_CompteBusinesService
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
            // _associesInscriptionBusinessService = associesInscriptionBusinessService;
            _identifiant_SIAuthentificationBusinesService = identifiant_SIAuthentificationBusinesService;
            _authentification_Erreur_CDE_CompteBusinesService = authentification_Erreur_CDE_CompteBusinesService;
        }

        public override Expression<Func<Inscription_Promoteur, object>>[] GetDefaultLoadProperties()
        {
            Expression<Func<Inscription_Promoteur, object>> loadCommune = x => x.Inscription_Commune;
            Expression<Func<Inscription_Promoteur, object>> loadGenre = x => x.Inscription_SexeId;

            return new Expression<Func<Inscription_Promoteur, object>>[] { loadCommune, loadGenre };
        }

        protected override void OnAdding(Inscription_Promoteur entity)
        {
            //Création compte dans la base [AUTHENTIFICATION]
            _ = entity.NUM_ACT_P.Trim().PadLeft(6, '0');
            var Num_Acte_Int = int.Parse(entity.NUM_ACT_P);

            var Num_Acte_Avec_Zéro = entity.NUM_ACT_P;
            var Num_Acte_Sans_Zéro = Num_Acte_Int.ToString();
            if (entity.NUM_ACT_P.Length < 6)
            {
                var n = 6 - entity.NUM_ACT_P.Length;
                for (int i = 0; i < n; i++)
                {
                    Num_Acte_Avec_Zéro = "0" + Num_Acte_Avec_Zéro;
                }
            }

            var commune_ele = _inscription_CommuneBusinesService.GetById(entity.CommuneId);
            var code_promoteur = commune_ele.Code + entity.DDN_P.Year + Num_Acte_Avec_Zéro;

            entity.Inscription_Commune = commune_ele;

            //Si le promoteur gérant existe déja dans sieje Base de données Suivi
            var promoteuractif = _promoteurSuiviBusinesService.GetByInformationsPromoteur(entity);

            if (promoteuractif.Count != 0)
            {
                throw new BusinessException("Vous possèdez déjà un dossier à NESDA.Veuillez bien vérifier votre date,commune  et  numéro d'acte de naissance.");
            }
            else
            {
                //Si le promoteur gérant existe déja dans inscription en ligne
                var promoteurExisteInscription = GetSingle(x => x.DDN_P == entity.DDN_P && x.CommuneId == entity.CommuneId && (x.NUM_ACT_P == Num_Acte_Avec_Zéro || x.NUM_ACT_P == Num_Acte_Sans_Zéro));

                if (promoteurExisteInscription != null)
                {
                    throw new BusinessException("Vous étes déjà inscrit dans notre espace.");
                }
                else
                {
                    var promoteurExisteNN = GetSingle(x => x.NIN == entity.NIN);

                    if (promoteurExisteNN != null)
                    {
                        throw new BusinessException("NINExist");
                    }
                    else
                    {
                        var ListCode_P = GetAllFiltered(o => o.DDN_P.Year == entity.DDN_P.Year && o.CommuneId == entity.CommuneId && (o.NUM_ACT_P == Num_Acte_Avec_Zéro || o.NUM_ACT_P == Num_Acte_Sans_Zéro));

                        if (ListCode_P.Count == 0)
                            code_promoteur += "0";
                        else
                        {
                            List<int> sequences = new List<int>();
                            foreach (var item in ListCode_P)
                            {
                                sequences.Add(int.Parse(item.CODE_P.Last().ToString()));
                            }
                            code_promoteur = code_promoteur + "" + (sequences.Max() + 1);
                        }
                    }
                }
            }

            entity.CODE_P = code_promoteur;

            base.OnAdding(entity);
        }

        public override BusinessResult Add(Inscription_Promoteur entity)
        {
            BusinessResult businessResult;

            try
            {
                Authentification_Compte entityCompte = Create_Compte(entity);

                OnAdding(entity);
                _repository.Add(entity);

                if (_unitOfWork.SaveChanges() == 0)
                {
                    throw new DataNotUpdatedException("Opération non enregsitrée niveau Promoteur!");
                }
                else
                {
                    // var resultCompteAdd = _compteAuthentificationBusinesService.Add(entityCompte);
                    // var resultCompteAdd = Add_Compte(entityCompte);

                    if (Add_Compte(entityCompte).Succeeded == false)
                    {
                        _repository.Delete(entity);
                        _unitOfWork.SaveChanges();
                        throw new DataNotUpdatedException("Opération non enregsitrée niveau Compte!");
                    }
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

        public virtual BusinessResult Add_Compte(Authentification_Compte entity)
        {
            BusinessResult businessResult = _compteAuthentificationBusinesService.Add(entity);

            return businessResult;
        }

        public virtual BusinessResult Add_Compte_CDE(Authentification_Compte_CDE entity)
        {
            BusinessResult businessResult = _authentification_Compt_CDEBusinesService.Add(entity);

            return businessResult;
        }

        public virtual BusinessResult Add_Identifiant_SI(Authentification_Compte entityCompte, Inscription_Detail_Dossier detail_Dossier, Inscription_Promoteur promoteur)
        {
            Authentification_Identifiant_SI entity = new();

            entity.Authentification_CompteId = entityCompte.Id;
            entity.Num_SI = detail_Dossier.Id.ToString();
            entity.Code_P = promoteur.CODE_P;

            BusinessResult businessResult = _identifiant_SIAuthentificationBusinesService.Add(entity);

            return businessResult;
        }

        public virtual BusinessResult Delete_Identifiant_SI(Authentification_Compte entityCompte, Inscription_Detail_Dossier detail_Dossier, Inscription_Promoteur promoteur)
        {
            Authentification_Identifiant_SI entity = _identifiant_SIAuthentificationBusinesService.GetSingle(x => x.Code_P == promoteur.CODE_P && x.Num_SI == detail_Dossier.Id.ToString() && x.Authentification_CompteId == entityCompte.Id);

            BusinessResult businessResult = _identifiant_SIAuthentificationBusinesService.Delete(entity);

            return businessResult;
        }

        private Authentification_Compte Create_Compte(Inscription_Promoteur entity)
        {
            Authentification_Compte entityCompte = new();

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
            //compte. = Id_Doss;

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

            return entityCompte;

        }

        public BusinessResult Attach_Identifiant_SI_Compte(Inscription_Promoteur entity,Inscription_Associes associes)
        {
            BusinessResult businessResult;

            try
            {
                Authentification_Compte entityCompte = _compteAuthentificationBusinesService.GetByUserName(entity.EMAIL_P);

                // Inscription_Associes associes = new();// _associesInscriptionBusinessService.GetSingle(x => x.Inscription_PromoteurId == entity.Id);

                Inscription_Detail_Dossier detail_Dossier = _detail_DossierInscriptionBusinessService.GetById(associes.Inscription_Detail_DossierId);

                if (Add_Identifiant_SI(entityCompte, detail_Dossier, entity).Succeeded == false)
                {
                    _repository.Delete(entity);
                    _unitOfWork.SaveChanges();
                    throw new DataNotUpdatedException("Opération non enregsitrée niveau Identifiant_SI_Compte!");
                }

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

        public BusinessResult Dettach_Identifiant_SI_Compte(Inscription_Promoteur entity,Inscription_Associes associes)
        {
            BusinessResult businessResult;

            try
            {
                Authentification_Compte entityCompte = _compteAuthentificationBusinesService.GetByUserName(entity.EMAIL_P);

                //Inscription_Associes associes = new();// _associesInscriptionBusinessService.GetSingle(x => x.Inscription_PromoteurId == entity.Id);

                Inscription_Detail_Dossier detail_Dossier = _detail_DossierInscriptionBusinessService.GetById(associes.Inscription_Detail_DossierId);

                if (Delete_Identifiant_SI(entityCompte, detail_Dossier, entity).Succeeded == false)
                {
                    throw new DataNotUpdatedException("Opération non enregsitrée niveau Identifiant_SI_Compte!");
                }

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

        public BusinessResult Attach_CDE_Compte(Inscription_Promoteur entity, CentreDeveloppementEntrepreneuriat cdeEle)
        {
            BusinessResult businessResult;
            Authentification_Erreur_CDE_Compte eleSavErreur = new();

            try
            {
                Authentification_Compte entityCompte = _compteAuthentificationBusinesService.GetByUserName(entity.EMAIL_P);

                Authentification_CDE authentification_CDE = _authentificationCDEBusinesService.GetByCodeNew(cdeEle.Code);

                eleSavErreur.Code = authentification_CDE.Code_CDE;
                eleSavErreur.DesignationAr = "CodeNew CentreDeveloppementEntrepreneuriat : " + cdeEle.Code;
                eleSavErreur.EMAIL_P = entity.EMAIL_P;

                _authentification_Erreur_CDE_CompteBusinesService.Add(eleSavErreur);

                Authentification_Compte_CDE authentification_Compte_CDE = new();

                authentification_Compte_CDE.Authentification_CDEId = authentification_CDE.Id;
                authentification_Compte_CDE.Authentification_CompteId = entityCompte.Id;

                if (Add_Compte_CDE(authentification_Compte_CDE).Succeeded == false)
                {
                    _repository.Delete(entity);
                    _unitOfWork.SaveChanges();
                    throw new DataNotUpdatedException("Opération non enregsitrée niveau CompteCDE!");
                }

                businessResult = BusinessResult.Success;
            }
            catch (DataNotUpdatedException ex)
            {
                eleSavErreur.Code = "DataUpExcp";
                eleSavErreur.EMAIL_P = entity.EMAIL_P;
                eleSavErreur.DesignationFr = ex.Message;
                _authentification_Erreur_CDE_CompteBusinesService.Add(eleSavErreur);

                businessResult = new BusinessResult(false);
                businessResult.Messages.Add(new MessageResult { Message = ex.Message, MessageType = MessageType.Warning });
            }
            catch (BusinessException ex)
            {
                eleSavErreur.Code = "BusinsExcp";
                eleSavErreur.EMAIL_P = entity.EMAIL_P;
                eleSavErreur.DesignationFr = ex.Message;
                _authentification_Erreur_CDE_CompteBusinesService.Add(eleSavErreur);

                businessResult = new BusinessResult(false);
                businessResult.Messages.Add(new MessageResult { Message = ex.Message, MessageType = MessageType.Warning });
            }
            catch (Exception ex)
            {
                eleSavErreur.Code = "Excp";
                eleSavErreur.EMAIL_P = entity.EMAIL_P;
                eleSavErreur.DesignationFr = ex.Message;
                _authentification_Erreur_CDE_CompteBusinesService.Add(eleSavErreur);

                businessResult = new BusinessResult(false);
                businessResult.Messages.Add(new MessageResult { Message = ex.Message, MessageType = MessageType.Warning });
            }

            return businessResult;
        }

        public BusinessResult ValidateNINUniqueness(string nin)
        {
            var existingPromoteur = _repository.GetSingle(x => x.NIN == nin);
            BusinessResult businessResult;

            if (existingPromoteur != null)
            {
                businessResult = new BusinessResult(false);
                businessResult.Messages.Add(new MessageResult { Message = "Ce numéro d'identification national est déjà utilisé", MessageType = MessageType.Error });
            }
            else
            {
                businessResult = BusinessResult.Success;
            }
            return businessResult;
        }

        public Inscription_Promoteur GetByNIN(string nin)
        {
            Inscription_Promoteur promoteur = new();

            promoteur = GetSingle(x => x.NIN == nin);

            return promoteur;
        }

        public Inscription_Promoteur GetByCode(string code)
        {
            Inscription_Promoteur promoteur = new();

            promoteur = GetSingle(x => x.CODE_P == code);

            return promoteur;
        }

        public bool CheckPromoteurExistenceByInfos(Inscription_Promoteur entity)
        {
            var commune_ele = _inscription_CommuneBusinesService.GetById(entity.CommuneId);

            entity.Inscription_Commune = commune_ele;

            //Si le promoteur gérant existe déja dans sieje Base de données Suivi
            var promoteuractif = _promoteurSuiviBusinesService.GetByInformationsPromoteur(entity);

            if (promoteuractif.Count != 0)
            {
                return true;
            }
            else
            {
                //Si le promoteur gérant existe déja dans inscription en ligne
                var promoteurExisteInscription = GetSingle(x => x.DDN_P == entity.DDN_P && x.CommuneId == entity.CommuneId && x.NUM_ACT_P == entity.NUM_ACT_P);

                if (promoteurExisteInscription != null)
                {
                    return true;
                }
                else
                {
                    var promoteurExisteNN = GetSingle(x => x.NIN == entity.NIN);

                    if (promoteurExisteNN != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public string CheckExtensionEligibilityByInfos(Inscription_Promoteur entity)
        {
            Inscription_Commune communeOld = new();

            // 1- Vérification par NIN.
            var listePromoteurs = GetAllFiltered(x => x.NIN == entity.NIN);

            if (listePromoteurs.Count != 0)
            {
                var listePromoteurExisteSuiviCode = _promoteurSuiviBusinesService.GetAllFilteredByCodePromoteurs(listePromoteurs, false);

                if (listePromoteurExisteSuiviCode.Count != 0)
                {
                    var promoteurNonExtNIN = listePromoteurExisteSuiviCode.First();

                    communeOld = _inscription_CommuneBusinesService.GetByCode(promoteurNonExtNIN.COMMUNE_NAIS);

                    return communeOld.Code;
                }
                else
                {
                    var listePromoteurExisteSuivi = _promoteurSuiviBusinesService.GetAllFilteredByCodePromoteurs(listePromoteurs, true);

                    if (listePromoteurExisteSuivi.Count != 0)
                    {
                        return ("يوجد ملف (مرحلة التوسع) مفعل في حسابك");
                    }
                }
            }

            // 2- Vérification par informations du promoteur
            communeOld = _inscription_CommuneBusinesService.GetById(entity.CommuneId);

            if (communeOld != null)
            {
                entity.Inscription_Commune = communeOld;
                var listePromoteurNonActifExtensionSuivi = _promoteurSuiviBusinesService.GetAllFilteredByInfosPromoteur(entity, false);

                if (listePromoteurNonActifExtensionSuivi.Count != 0)
                {
                    var promoteurNonExt = listePromoteurNonActifExtensionSuivi.First();

                    communeOld = _inscription_CommuneBusinesService.GetByCode(promoteurNonExt.COMMUNE_NAIS);

                    return communeOld.Code;
                }
                else
                {
                    var listePromoteurActifExtensionSuivi = _promoteurSuiviBusinesService.GetAllFilteredByInfosPromoteur(entity, true);

                    if (listePromoteurActifExtensionSuivi.Count != 0)
                    {
                        return ("يوجد ملف (مرحلة التوسع) مفعل في حسابك");
                    }
                    else
                    {
                        return ("لا يوجد أي ملف (مرحلة الإنشاء) مفعل في حسابك");
                    }
                }
            }
            else
            {
                return ("نواجه حاليًا عطلًا تقنيًا. نرجو منكم التكرم بالتواصل مع مسؤول النظام");
            }
        }

        public string ConstructMatriculeSieje(Inscription_Promoteur promoteur)
        {
            string matricule = "", annee, squentialNumber, codeCommune, numeroActeNaissance;
            annee = promoteur.DDN_P.Year.ToString();

            numeroActeNaissance = "0" + promoteur.NUM_ACT_P;

            squentialNumber = GetSequentialNumber(promoteur);
            if (promoteur.IsNaissanceEtrange()) codeCommune = "9999";
            else
            {
                var commune_ele = _inscription_CommuneBusinesService.GetById(promoteur.CommuneId);
                codeCommune = commune_ele.Code;
            }

            matricule = matricule.StringWritter(codeCommune, annee, numeroActeNaissance.Trim(), squentialNumber);
            return matricule;
        }

        public string GetSequentialNumber(Inscription_Promoteur promoteur)
        {
            const int oneDigit = 1;
            const int twoDigits = 2;
            string sequentialNumber = "";
            int countSequential;

            if (promoteur.IsNaissanceEtrange())
            {
                countSequential = _repository.Count(CodeEtrangeExiste(promoteur));
                _ = countSequential.CountDigit() switch
                {
                    oneDigit => sequentialNumber.StringWritter("000", countSequential.ToString()),
                    twoDigits => sequentialNumber.StringWritter("00", countSequential.ToString()),
                    _ => sequentialNumber.StringWritter("0", countSequential.ToString()),
                };
            }
            else countSequential = _repository.Count(CodeLocalExiste(promoteur));
            return countSequential.ToString();
        }

        public void OnAddingPromoteur(Inscription_Promoteur entity)
        {
            OnAdding(entity);
        }

        #region Conditions on promoteur
        private static Expression<Func<Inscription_Promoteur, bool>> CodeEtrangeExiste(Inscription_Promoteur promoteur)
        {
            return (x => x.DDN_P == promoteur.DDN_P
                      && x.CommuneId == 1543 // Id=1543	Code=9999	ETRANGER
                      && x.Inscription_SexeId == promoteur.Inscription_SexeId);
        }
        private static Expression<Func<Inscription_Promoteur, bool>> CodeLocalExiste(Inscription_Promoteur promoteur)
        {
            return (x => x.DDN_P.Year == promoteur.DDN_P.Year
                      && x.CommuneId == promoteur.CommuneId
                      && x.Inscription_SexeId == promoteur.Inscription_SexeId);
        }
        #endregion

    }
}
