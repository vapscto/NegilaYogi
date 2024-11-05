using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace DomainModel.Model.com.vapstech.Portals.Student
{
    [Table("IVRM_Student_CovidTest")]
    public class Student_CovidTestDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISTUCOVTST_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public DateTime? ISTUCOVTST_TestDate { get; set; }
        public string ISTUCOVTST_TestResult { get; set; }
        public string ISTUCOVTST_FileName { get; set; }
        public string ISTUCOVTST_FilePath { get; set; }
        public bool ISTUCOVTST_ActiveFlag { get; set; }
        public DateTime ISTUCOVTST_CreatedDate { get; set; }
        public DateTime ISTUCOVTST_UpdatedDate { get; set; }
        public long ISTUCOVTST_CreatedBy { get; set; }
        public long ISTUCOVTST_UpdatedBy { get; set; }
    }
}
