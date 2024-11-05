using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Candidate_Experience")]
    public class HR_Candidate_ExperienceDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRCEXP_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRCD_Id { get; set; }
        public string HRCEXP_CompanyName { get; set; }
        public string HRCEXP_Designation { get; set; }
        public DateTime? HRCEXP_From { get; set; }
        public DateTime? HRCEXP_To { get; set; }
        public decimal HRCEXP_Salary { get; set; }
        public bool HRCEXP_ActiveFlag { get; set; }
        public long HRCEXP_CreatedBy { get; set; }
        public long HRCEXP_UpdatedBy { get; set; }
    }
}
