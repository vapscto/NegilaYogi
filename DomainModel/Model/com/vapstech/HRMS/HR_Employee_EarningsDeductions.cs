using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Employee_EarningsDeductions")]
    public class HR_Employee_EarningsDeductions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HREED_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long HRMED_Id { get; set; }
        public decimal? HREED_Amount { get; set; }
        public string HREED_Percentage { get; set; }
        public bool? HREED_ActiveFlag { get; set; }
        public bool? HREED_MaxApplicableFlg { get; set; }
        public decimal? HREED_ApplicableMaxValue { get; set; }

    }
}
