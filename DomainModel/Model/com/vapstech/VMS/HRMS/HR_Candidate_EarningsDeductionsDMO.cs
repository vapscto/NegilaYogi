using DomainModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("HR_Candidate_EarningsDeductions")]
    public class HR_Candidate_EarningsDeductionsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRCED_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRCD_Id { get; set; }
        public long HRMED_Id { get; set; }
        public decimal HRCED_Amount { get; set; }
        public string HRCED_Percentage { get; set; }
        public bool HRCED_ActiveFlag { get; set; }
    }

}