using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.admission
{
    [Table("Adm_m_School_Categorywise_Documents")]
    public class MasterDocumentMappingDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PASCD_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMSMD_Id { get; set; }
        public long AMC_Id { get; set; }

        // public ICollection<pagemodulemapping> modulepagemapping { get; set; }
    }
}
