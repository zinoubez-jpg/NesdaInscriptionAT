using Anade.Domain.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anade.Khadamat.Domain.Entity
{
    public partial class Consultation : IEntity<int>
    {
        public int Id { get; set; }
        public string Structure { get; set; }
        public string DiscriptionAr { get; set; }
        public string DiscriptionFr { get; set; }
        public DateTime DatePublication { get; set; }
        [Display(Name = "CopieConsultationPath")]
        public string CopieConsultationPath { get; set; }
    }
}
