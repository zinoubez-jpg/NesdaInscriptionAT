namespace Anade.Khadamat.Domain
{
    using Anade.Domain.Core;

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Suivi_Formation : IEntity<int>
    {

        public int Id { get; set; }
        public string NIN { get; set; }
        public string Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateDebutSessionFormation { get; set; }

    }
}
