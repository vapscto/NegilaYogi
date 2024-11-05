using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class TTClassMasterDTO
    {
        public bool returnval;
        public string returnduplicatestatus;
     
        public long TTFPD_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long HRME_Id { get; set; }
        public int TTFPD_TotWeekPeriods { get; set; }
        public bool TTFPD_ActiveFlag { get; set; }

        public long TTFPDD_Id { get; set; }
        public int TTFPD_StaffConsecutive { get; set; }
        public long TTMC_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public int TTFPD_TotalPeriods { get; set; }
        public int TTFPD_AllotedPeriods { get; set; }
        public int TTFPD_AvailablePeriods { get; set; }
        public bool TTFPDD_ActiveFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long NOP { get; set; }
        public string ASMAY_Year { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string TTMD_DayName { get; set; }
        public string TTMC_CategoryName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string staffName { get; set; }
        public string ISMS_SubjectName { get; set; }
        public TTClassMasterDTO[] TempararyArrayList { get; set; }
      //  public TTClassMasterDTO[] period_distri_list { get; set; }

        public TTClassMasterDTO[] all_period_distri_list { get; set; }
        public Array period_distri_edit { get; set; }
        public Array period_distri_detail_edit { get; set; }
        public TTClassMasterDTO[] detailspopuparray { get; set; }

        public TTClassMasterDTO[] TempararyArray { get; set; }
      
        public TTClassMasterDTO[] All_list { get; set; }
        public TTClassMasterDTO[] Temp_class_Array { get; set; }       
        public Array periodlistedit { get; set; }

        //for this only
        public int edit_count { get; set; }      
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
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

    }
}
