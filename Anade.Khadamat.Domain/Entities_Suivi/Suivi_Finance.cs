namespace Anade.Khadamat.Domain
{
    using Anade.Domain.Core;

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Suivi_Finance : IEntity<int>
    {

        public int Id { get; set; }
        public string Num_Doss { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date_Finance { get; set; }

    }
}
