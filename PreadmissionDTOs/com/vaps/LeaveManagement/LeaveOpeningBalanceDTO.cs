using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.LeaveManagement
{
    public class LeaveOpeningBalanceDTO : CommonParamDTO
    {        
        public long HRMED_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMEDT_Id { get; set; }
        public string HRMED_EarnDedName { get; set; }
        public string HRMED_EarnDedTypeFlag { get; set; }
        public string HRMED_AmountPercentFlag { get; set; }
        public string HRMED_AmountPercent { get; set; }
        public bool HRMED_ActiveFlag { get; set; }
        public decimal HRMED_RoundOffFlag { get; set; }
        public DateTime HRMED_EntryDate { get; set; }
        public long LoginId { get; set; }

        
    }
}
