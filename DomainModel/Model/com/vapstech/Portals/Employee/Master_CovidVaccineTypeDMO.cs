using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("IVRM_Master_CovidVaccineType")]
    public class Master_CovidVaccineTypeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IMCOVVAC_Id { get; set; }
      public long? AMST_Id { get; set; }
        public string IMCOVVAC_VaccinationName { get; set; }
        public bool IMCOVVAC_ActiveFlag { get; set; }
        public DateTime IMCOVVAC_CreatedDate { get; set; }
        public DateTime IMCOVVAC_UpdatedDate { get; set; }
        public long IMCOVVAC_CreatedBy { get; set; }
        public long IMCOVVAC_UpdatedBy { get; set; }
    }

}
