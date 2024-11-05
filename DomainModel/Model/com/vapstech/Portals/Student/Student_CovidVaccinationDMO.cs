using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace DomainModel.Model.com.vapstech.Portals.Student
{
    [Table("IVRM_Student_CovidVaccination")]
    public class Student_CovidVaccinationDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISTUCOVVAC_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long IMCOVVAC_Id { get; set; }
        public DateTime? ISTUCOVVAC_VaccinationDate { get; set; }
        public string ISTUCOVVAC_Dose { get; set; }
        public string ISTUCOVVAC_FileName { get; set; }
        public string ISTUCOVVAC_FilePath { get; set; }
        public bool ISTUCOVVAC_ActiveFlag { get; set; }
        public DateTime ISTUCOVVAC_CreatedDate { get; set; }
        public DateTime ISTUCOVVAC_UpdatedDate { get; set; }
        public long ISTUCOVVAC_CreatedBy { get; set; }
        public long ISTUCOVVAC_UpdatedBy { get; set; }
    }
}
