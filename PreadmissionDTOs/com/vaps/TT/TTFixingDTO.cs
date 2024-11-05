using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class TTFixingDTO
    {
        public bool returnval;
        public string returnduplicatestatus;     
        public string returnrestrictstatus;

        //1
        public long TTFDP_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long TTMC_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long TTMD_Id { get; set; }
        public int TTMP_Id { get; set; }
        public string TTFDP_AllotedFlag { get; set; }
        public bool TTFDP_ActiveFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        //2
        public long TTFDS_Id { get; set; }
        public string TTFDS_AllotedFlag { get; set; }
        public bool TTFDS_ActiveFlag { get; set; }
        public bool TTFDS_SUbSelcFlag { get; set; }

        public long TTFDSCC_Id { get; set; }
        public long TTFDSCC_Periods { get; set; }
        public bool TTFDSCC_ActiveFlag { get; set; }


        //3
        public long TTFDSU_Id { get; set; }      
        public string TTFDSU_AllotedFlag { get; set; }
        public bool TTFDSU_ActiveFlag { get; set; }
        public bool TTFDSU_SUbSelcFlag { get; set; }

        public long TTFDSUCC_Id { get; set; }
        public long TTFDSUCC_Periods { get; set; }
        public bool TTFDSUCC_ActiveFlag { get; set; }

        //4
        public long TTFPS_Id { get; set; }      
        public string TTFPS_AllotedFlag { get; set; }
        public bool TTFPS_ActiveFlag { get; set; }
        public bool TTFPS_SUbSelcFlag { get; set; }

        public long TTFPSCC_Id { get; set; }
        public long TTFPSCC_Days { get; set; }
        public bool TTFPSCC_ActiveFlag { get; set; }

        //5
        public long TTFPSU_Id { get; set; }     
        public string TTFPSU_AllotedFlag { get; set; }
        public bool TTFPSU_ActiveFlag { get; set; }
        public bool TTFPSU_SUbSelcFlag { get; set; }

        public long TTFPSUCC_Id { get; set; }
        public long TTFPSUCC_Days { get; set; }
        public bool TTFPSUCC_ActiveFlag { get; set; }
       
        //for this only
        public Array acayear { get; set; }
        public Array categorylist { get; set; }
        public Array classlist { get; set; }
        public Array sectionlist { get; set; }
        public Array stafflist { get; set; }
        public Array subjectlist { get; set; }
        public Array periodlist { get; set; }
        public Array daylist { get; set; }
        public Array categorybyyear { get; set; }
        public Array classbycategory { get; set; }
        public Array periodbyclass { get; set; }
        public Array sectionbyclass { get; set; }
        public Array staffbyall { get; set; }
        public Array subjectbystaff { get; set; }
        public Array clssbystaff { get; set; }
        public Array secsbystaff { get; set; }
        public Array subsbystaff { get; set; }

        public Array clssbysub { get; set; }
        public Array secsbysub { get; set; }
        public Array staffbysub { get; set; }

        public string ASMAY_Year { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string TTMD_DayName { get; set; }
        public string TTMC_CategoryName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string staffName { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string TTMP_PeriodName { get; set; }
        
        public TTFixingDTO[] fix_day_period_list { get; set; }
        public TTFixingDTO[] all_fix_day_staff_list { get; set; }
        public TTFixingDTO[] all_fix_day_subject_list { get; set; }
        
        public TTFixingDTO[] all_fix_period_staff_list { get; set; }
        public TTFixingDTO[] all_fix_period_subject_list { get; set; }

        public TTFixingDTO[] TempararyArrayList { get; set; }
       // public TTFixingDTO[] all_period_distri_list { get; set; }
        public Array fix_day_period_edit { get; set; }
        public Array fix_day_staff_edit { get; set; }       
        public Array fix_day_staff__classecedit { get; set; }
        public Array fix_day_subject_edit { get; set; }
        public Array fix_day_subject__classecedit { get; set; }   
        public Array fix_period_staff_edit { get; set; }
        public Array fix_period_staff__classecedit { get; set; }
        public Array fix_period_subject_edit { get; set; }
        public Array fix_period_subject__classecedit { get; set; }
        // public Array period_distri_edit { get; set; }
        //public Array period_distri_detail_edit { get; set; }
        public TTFixingDTO[] detailspopuparray2 { get; set; }
        public TTFixingDTO[] detailspopuparray3 { get; set; }
        public TTFixingDTO[] detailspopuparray4 { get; set; }
        public TTFixingDTO[] detailspopuparray5 { get; set; }
        public TTFixingDTO[] TempararyArray { get; set; }

        public TTFixingDTO[] All_list { get; set; }
        public TTFixingDTO[] Temp_class_Array { get; set; }
        public Array periodlistedit { get; set; }



        //some
        public long TTFPD_Id { get; set; }   
        public int TTFPD_TotWeekPeriods { get; set; }
        public bool TTFPD_ActiveFlag { get; set; }

        public long TTFPDD_Id { get; set; }
     //   public long TTFPD_Id { get; set; }      
        public int TTFPD_TotalPeriods { get; set; }
        public int TTFPD_AllotedPeriods { get; set; }
        public int TTFPD_AvailablePeriods { get; set; }
        public bool TTFPDD_ActiveFlag { get; set; }   
        public long NOP { get; set; }
        public long NOD { get; set; }

        public long period_count { get; set; }
        public long day_count { get; set; }

        public long minvalue { get; set; }
        public long maxvalue { get; set; }

        public int ttperiod { get; set; }
        public int ttmpid { get; set; }






    }
}
