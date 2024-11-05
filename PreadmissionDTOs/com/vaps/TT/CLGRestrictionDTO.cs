using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class CLGRestrictionDTO
    {
        public bool returnval;
        public string returnduplicatestatus;     
        public string returnfixstatus;

        //1
        public long TTRDP_Id { get; set; }
        public long TTRDPC_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long TTMC_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long TTMD_Id { get; set; }
        public int TTMP_Id { get; set; }
        public string TTRDP_AllotedFlag { get; set; }
        public bool TTRDP_ActiveFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        //2
        public long TTRDS_Id { get; set; }
        public string TTRDS_AllotedFlag { get; set; }
        public bool TTRDS_ActiveFlag { get; set; }
        public bool TTRDS_SUbSelcFlag { get; set; }

        public long TTRDSCC_Id { get; set; }
        public long TTRDSCC_Periods { get; set; }
        public bool TTRDSCC_ActiveFlag { get; set; }


        //3
        public long TTRDSU_Id { get; set; }      
        public string TTRDSU_AllotedFlag { get; set; }
        public bool TTRDSU_ActiveFlag { get; set; }
        public bool TTRDSU_SUbSelcFlag { get; set; }

        public long TTRDSUCC_Id { get; set; }
        public long TTRDSUCC_Periods { get; set; }
        public bool TTRDSUCC_ActiveFlag { get; set; }

        //4
        public long TTRPS_Id { get; set; }      
        public string TTRPS_AllotedFlag { get; set; }
        public bool TTRPS_ActiveFlag { get; set; }
        public bool TTRPS_SUbSelcFlag { get; set; }

        public long TTRPSCC_Id { get; set; }
        public long TTRPSCC_Days { get; set; }
        public bool TTRPSCC_ActiveFlag { get; set; }

        //5
        public long TTRPSU_Id { get; set; }     
        public string TTRPSU_AllotedFlag { get; set; }
        public bool TTRPSU_ActiveFlag { get; set; }
        public bool TTRPSU_SUbSelcFlag { get; set; }

        public long TTRPSUCC_Id { get; set; }
        public long TTRPSUCC_Days { get; set; }
        public bool TTRPSUCC_ActiveFlag { get; set; }
       
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
        public Array academiclist { get; set; }
        public Array daydropdown { get; set; }
        public Array semisterlist { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public Array clssbysub { get; set; }
        public Array secsbysub { get; set; }
        public Array staffbysub { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }

        public string ASMAY_Year { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string TTMD_DayName { get; set; }
        public string TTMC_CategoryName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string staffName { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string TTMP_PeriodName { get; set; }
        
        public Array restrict_day_period_list { get; set; }
        public CLGRestrictionDTO[] all_restrict_day_staff_list { get; set; }
        public CLGRestrictionDTO[] all_restrict_day_subject_list { get; set; }
        
        public CLGRestrictionDTO[] all_restrict_period_staff_list { get; set; }
        public CLGRestrictionDTO[] all_restrict_period_subject_list { get; set; }

        public CLGRestrictionDTO[] TempararyArrayList { get; set; }
       // public CLGRestrictionDTO[] all_period_distri_list { get; set; }
        public Array restrict_day_period_edit { get; set; }
        public Array restrict_day_staff_edit { get; set; }       
        public Array restrict_day_staff__classecedit { get; set; }
        public Array restrict_day_subject_edit { get; set; }
        public Array restrict_day_subject__classecedit { get; set; }   
        public Array restrict_period_staff_edit { get; set; }
        public Array restrict_period_staff__classecedit { get; set; }
        public Array restrict_period_subject_edit { get; set; }
        public Array restrict_period_subject__classecedit { get; set; }
        // public Array period_distri_edit { get; set; }
        //public Array period_distri_detail_edit { get; set; }
        public Array detailspopuparray2 { get; set; }
        public Array detailspopuparray3 { get; set; }
        public Array detailspopuparray4 { get; set; }
        public Array detailspopuparray5 { get; set; }
        public CLGRestrictionDTO[] TempararyArray { get; set; }

        public CLGRestrictionDTO[] All_list { get; set; }
        public CLGRestrictionDTO[] Temp_class_Array { get; set; }
        public Array periodlistedit { get; set; }



        //some
        public long TTFPD_Id { get; set; }   
        public int TTFPD_TotWeekPeriods { get; set; }
        public bool TTFPD_ActiveFlag { get; set; }

        public long TTFPDD_Id { get; set; }
     //   public long TTRPD_Id { get; set; }      
        public int TTFPD_TotalPeriods { get; set; }
        public int TTFPD_AllotedPeriods { get; set; }
        public int TTFPD_AvailablePeriods { get; set; }
        public bool TTFPDD_ActiveFlag { get; set; }   
        public long NOP { get; set; }
        public long NOD { get; set; }

        public long period_count { get; set; }
        public long day_count { get; set; }
        //  public CLGRestrictionDTO[] period_distri_list { get; set; }








    }
}
