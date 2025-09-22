using System;

namespace Anade.Khadamat.Domain
{
    // Classe DTO pour l'exposition des données
    public class SuiviEvenementDto
    {
        public string Numero { get; set; }
        public DateTime? Datesignature { get; set; }
        public string Objet { get; set; }
    }
}
