using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("IVRM_Staff_CovidTest")]
    public class Staff_CovidTestDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISTCOVTST_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime? ISTCOVTST_TestDate { get; set; }
        public string ISTCOVTST_TestResult { get; set; }
        public string ISTCOVTST_FileName { get; set; }
        public string ISTCOVTST_FilePath { get; set; }
        public bool ISTCOVTST_ActiveFlag { get; set; }
        public DateTime ISTCOVTST_CreatedDate { get; set; }
        public DateTime ISTCOVTST_UpdatedDate { get; set; }
        public long ISTCOVTST_CreatedBy { get; set; }
        public long ISTCOVTST_UpdatedBy { get; set; }
    }
}
