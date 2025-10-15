using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anade.Khadamat.Domain
{
    using Anade.Domain.Core;

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Suivi_Evenement : IEntity<int>
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ajoutez ceci
        public int Id { get; set; }

        [ForeignKey("PROMOTEUR")] // Ajoutez cet attribut
        public string Code_P { get; set; }
        public string Num_Doss { get; set; }

        public Nullable<System.DateTime> Date { get; set; }
        public string Evenement { get; set; }
        public string Observation { get; set; }

        public virtual Suivi_Promoteur PROMOTEUR { get; set; }

    }
}
