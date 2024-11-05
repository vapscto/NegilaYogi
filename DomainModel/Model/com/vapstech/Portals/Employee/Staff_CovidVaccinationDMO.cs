using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("IVRM_Staff_CovidVaccination")]
    public class Staff_CovidVaccinationDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISTCOVVAC_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long IMCOVVAC_Id { get; set; }
        public DateTime? ISTCOVVAC_VaccinationDate { get; set; }
        public string ISTCOVVAC_Dose { get; set; }
        public string ISTCOVVAC_FileName { get; set; }
        public string ISTCOVVAC_FilePath { get; set; }
        public bool ISTCOVVAC_ActiveFlag { get; set; }
        public DateTime ISTCOVVAC_CreatedDate { get; set; }
        public DateTime ISTCOVVAC_UpdatedDate { get; set; }
        public long ISTCOVVAC_CreatedBy { get; set; }
        public long ISTCOVVAC_UpdatedBy { get; set; }
    }
}
