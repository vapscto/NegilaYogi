using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeYearlyRedateSettingDTO
    {

        public long MI_Id { get; set; }

        public long user_id { get; set; }
        public string returnduplicatestatus { get; set; }
        public Array GroupHeadData { get; set; }
        public Array academicdrp { get; set; }

        public bool returnval { get; set; }
        public bool dupr { get; set; }
        public string message { get; set; }

        public long FYREBSET_Id { get; set; }
      
        public long ASMAY_Id { get; set; }
        public string FYREBSET_RebateTypeFlg { get; set; }
        public DateTime FYREBSET_RebateDate { get; set; }
        public Decimal FYREBSET_RebateAmtOrPercentValue { get; set; }
        public bool FYREBSET_ActiveId { get; set; }
        public string ASMAY_Year { get; set; }

    }
}
