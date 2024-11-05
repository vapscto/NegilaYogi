using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.admission
{
    [Table("Adm_School_M_Section")]
    public class School_M_Section :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ASMS_Id { get; set; }

        public long MI_Id { get; set; }
        public string ASMC_SectionName { get; set; }
        public string ASMC_SectionCode { get; set; }
        public int ASMC_Order { get; set; }
        public int ASMC_ActiveFlag { get; set; }
        public int ASMC_MaxCapacity { get; set; }


    }
}
