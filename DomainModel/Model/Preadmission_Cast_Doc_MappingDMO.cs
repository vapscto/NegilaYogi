using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Preadmission_Cast_Doc_Mapping")]
    public class Preadmission_Cast_Doc_MappingDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PCDM_Id { get; set; }
        public long AMSMD_Id { get; set; }
        public long IMC_Id { get; set; }
        public long MI_Id { get; set; }
    }
}
