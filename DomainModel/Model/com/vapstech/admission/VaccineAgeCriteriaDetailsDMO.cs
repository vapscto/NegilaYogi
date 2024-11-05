using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.admission
{
    [Table("Adm_Student_Vaccine_AgeCriteria_Details")]
    public class VaccineAgeCriteriaDetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ASVACD_Id { get; set; }
        public long ASVAC_Id { get; set; }
        public string ASVACD_VaccineName { get; set; }
        public string ASVACD_VaccineType { get; set; }
        public bool ASVACD_ActiveFlag { get; set; }
        public DateTime ASVACD_CreatedDate { get; set; }
        public DateTime ASVACD_UpdatedDate { get; set; }
    }
}
