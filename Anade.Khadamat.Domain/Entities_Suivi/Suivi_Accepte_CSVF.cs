namespace Anade.Khadamat.Domain
{
    using Anade.Domain.Core;

    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class Suivi_Accepte_CSVF : IEntity<int>
    {

        public int Id { get; set; }
        public string Num_Doss { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date_csvf { get; set; }
        
    }
}
