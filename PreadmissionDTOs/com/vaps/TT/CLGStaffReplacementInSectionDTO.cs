using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class CLGStaffReplacementInSectionDTO
    {


        public bool returnval { get; set; }

        public long MI_Id { get; set; }
        public long TTMC_Id { get; set; }
        public string TTMC_CategoryName { get; set; }
        public long ASMAY_Id { get; set; }
        public long TTMD_Id { get; set; }
        public int TTMP_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public decimal TTMBC_AfterPeriod { get; set; }
        public string TTMBC_BreakName { get; set; }
        public Array catelist { get; set; }
        public Array academiclist { get; set; }
        public Array periodslst { get; set; }
        public Array datalst { get; set; }
        public Array classbycategory { get; set; }
        public Array staffDrpDwn { get; set; }
        public string staffNamelst { get; set; }
        public Array gridweeks { get; set; }
        public Array Break_list { get; set; }
        public Array Break_list_all { get; set; }
        public Array classlist { get; set; }
        public Array sectionlist { get; set; }
        public string TTMSUAB_Abbreviation { get; set; }
        //by mahaboob
        public string ASMCL_ClassName { get; set; }
     
       
        public string ASMC_SectionName { get; set; }
        public string TTMD_DayName { get; set; }
        public string TTMP_PeriodName { get; set; }
        public long TTFG_Id { get; set; }
        public long TTFGD_Id { get; set; }
        public string staffName { get; set; }
        public string ISMS_SubjectName { get; set; }
        public Array Time_Table { get; set; }

        public Array Data_lst { get; set; }
        public long staffSDK { get; set; }
        public long subSDK { get; set; }
        public long conSDK { get; set; }

        public long TTMD_ID_from { get; set; }
        public long TTMP_ID_from { get; set; }
        public long TTMD_ID_to { get; set; }
        public long TTMP_ID_to { get; set; }

    }
}
