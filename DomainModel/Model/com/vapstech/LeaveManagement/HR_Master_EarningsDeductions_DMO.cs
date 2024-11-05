using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.LeaveManagement
{
    [Table("HR_Master_EarningsDeductions")]
    public class HR_Master_EarningsDeductions_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
             
        public long HRMED_Id { get; set; }
        public long MI_Id { get; set; }
        public long? HRMEDT_Id { get; set; }
        public string HRMED_Name { get; set; }
        public string HRMED_EDTypeFlag { get; set; }
        public string HRMED_EarnDedFlag { get; set; }
        public string HRMED_AmountPercentFlag { get; set; }
        public string HRMED_AmountPercent { get; set; }
        public bool HRMED_ActiveFlag { get; set; }
        public string HRMED_RoundOffFlag { get; set; }
        public DateTime? HRMED_EntryDate { get; set; }        
        public long? LoginId { get; set; }
    }
}
