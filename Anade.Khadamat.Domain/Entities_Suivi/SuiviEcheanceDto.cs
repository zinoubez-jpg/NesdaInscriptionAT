using System;

namespace Anade.Khadamat.Domain
{
    // Classe DTO pour l'exposition des données
    public class SuiviEcheanceDto
    {
        public string Numero { get; set; }
        public DateTime? Dateecheance { get; set; }
        public float Montantecheance { get; set; }
        public DateTime? Datepaiement { get; set; }
        public float? Montantpaye { get; set; }
        public double? Montantreliquat { get; set; }
    }
}