using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
  [Table("HR_Master_EarningsDeductions")]
  public class HR_Master_EarningsDeductions
    {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long HRMED_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMED_Name { get; set; }
        public string HRMED_EDTypeFlag { get; set; }
        public string HRMED_EarnDedFlag { get; set; }
        public string HRMED_AmountPercentFlag { get; set; }
        public string HRMED_AmountPercent { get; set; }
        public bool HRMED_ActiveFlag { get; set; }
        public string HRMED_RoundOffFlag { get; set; }
        public long? HRMED_Order { get; set; }
        public bool HRMED_ReviseFlg { get; set; }
    }
}
