using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class TTPeriodAllocationDTO
    {
       public int  count { get; set; }
        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }
        //for period
        public int TTMP_Id { get; set; }
        public long MI_Id { get; set; }
        public string TTMP_PeriodName { get; set; }
        public bool TTMP_ActiveFlag { get; set; }
        //for classwise period
        public long TTMPC_Id { get; set; }
      //public int TTMP_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public bool TTMPC_ActiveFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }


        public long TTMC_Id { get; set; }
        public long TT_Day { get; set; }
        public long TT_Period { get; set; }
        public string TTMDPT_StartTime { get; set; }
        public string TTMDPT_EndTime { get; set; }
       
      //  public long ASMCL_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMAY_Year { get; set; }

        public string TTMD_DayName { get; set; }
     //   public long TTMC_Id { get; set; }
        public string TTMC_CategoryName { get; set; }
        public TTPeriodAllocationDTO[] TempararyArray { get; set; }
        public Array TempararyArray_categ { get; set; }
        public TTPeriodAllocationDTO[] All_list { get; set; }
        public TTPeriodAllocationDTO[] temp_period_Array { get; set; }
        public TTPeriodAllocationDTO[] Temp_class_Array { get; set; }
        public TTPeriodAllocationDTO[] Edit_period_class { get; set; }
  
        public Array periodlist { get; set; }
        public Array periodlist_class { get; set; }
        public TTPeriodAllocationDTO[] tempperiods { get; set; }
        public Array catelist { get; set; }
        public Array day_list { get; set; }
        public Array class_list { get; set; }
        public Array periodlistedit { get; set; }
        public Array acayear { get; set; }
        public int period_count { get; set; }

        public string comparevlue { get; set; }
        public bool cannot { get; set; }
    }
}
