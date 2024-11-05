using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_Master_EarningsDeductionsDTO
    {
    public long HRMED_Id { get; set; }
    public long MI_Id { get; set; }
    public string HRMED_Name { get; set; }
    public string HRMED_EDTypeFlag { get; set; }
    public string HRMED_EarnDedFlag { get; set; }
    public string HRMED_AmountPercentFlag { get; set; }
    public string HRMED_AmountPercent { get; set; }
    public bool HRMED_ActiveFlag { get; set; }
    public string HRMED_RoundOffFlag { get; set; }
    public Array earningdetectionList { get; set; }
    public string retrunMsg { get; set; }
    public long roleId { get; set; }
    public bool HRMED_ReviseFlg { get; set; }

    public Array earningList { get; set; }
    public Array detectionList { get; set; }

    public Array eardettypelist { get; set; }

    public Array eardettypeDropdown { get; set; }

        public HR_Master_EarningsDeductionsPerDTO[] perc_OfDTO { get; set; }
        public Array selectedearningdetectionList { get; set; }
        public string percentOff { get; set; }




        public decimal? HREED_Amount { get; set; }
        public Array arrearlist { get; set; }

        public Array grosslist { get; set; }

        public HR_Master_EarningsDeductionsDTO[] EarningsDeductionsDTO { get; set; }

        public long? HRMED_Order { get; set; }




    }
}
