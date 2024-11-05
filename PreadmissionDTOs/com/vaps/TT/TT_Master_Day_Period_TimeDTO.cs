using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class TT_Master_Day_Period_TimeDTO:CommonParamDTO
    {
        public long TTMDPT_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long TTMC_Id { get; set; }
        public long TTMD_Id { get; set; }
        public int TTMP_Id { get; set; }
        public string TTMDPT_StartTime { get; set; }
        public string TTMDPT_EndTime { get; set; }
        public bool TTMDPT_ActiveFlag { get; set; }
        public int dupcnt { get; set; }
        public int sucnt { get; set; }
        public TT_Master_Day_Period_TimeDTO[] datidss { get; set; }
        public Array academicdrp { get; set; }

        public Array categorylist { get; set; }

        public Array daydrp { get; set; }

        public Array perioddrp { get; set; }

        public Array gridview { get; set; }
        public string ASMAY_Year { get; set; }
        public string AMC_Name { get; set; }

        public string TTMD_DayName { get; set; }

        public string TTMP_PeriodName { get; set; }
        public string TTMC_CategoryName { get; set; }

        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }
        public Array periodlistedit { get; set; }
    }
}
