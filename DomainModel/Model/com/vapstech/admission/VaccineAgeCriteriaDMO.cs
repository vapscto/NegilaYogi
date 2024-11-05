using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.admission
{
    [Table("Adm_Student_Vaccine_AgeCriteria")]
    public class VaccineAgeCriteriaDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ASVAC_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASVAC_AgeStartNo { get; set; }
        public long ASVAC_AgeEndNo { get; set; }
        public bool ASVAC_ActiveFlag { get; set; }
        public DateTime ASVAC_CreatedDate { get; set; }
        public DateTime ASVAC_UpdatedDate { get; set; }
        public List<VaccineAgeCriteriaDetailsDMO> VaccineAgeCriteriaDetailsDMO { get; set; }
    }
}
