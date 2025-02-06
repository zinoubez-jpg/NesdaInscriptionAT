using Anade.Domain.Core;
using System.Collections.Generic;

namespace Anade.Khadamat.Identity
{
    public partial class Structure : IEntity<int>
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Structure()
        {

            Children = new HashSet<Structure>();

            Users = new HashSet<UserStructure>();

        }

        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string CodeStructure { get; set; }
        public string Designation { get; set; }
        public virtual Niveau Niveau { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Structure> Children { get; set; }
        public virtual Structure Parent { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserStructure> Users { get; set; }

    }
}
