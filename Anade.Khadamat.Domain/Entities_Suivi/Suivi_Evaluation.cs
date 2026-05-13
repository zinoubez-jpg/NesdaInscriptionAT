
namespace Anade.Khadamat.Domain
{
    using Anade.Domain.Core;

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Suivi_Evaluation : IEntity<int>
    {

        public int Id { get; set; }
        public string PromoteurId { get; set; }
        public int ScoreTotal { get; set; }

    }
}
