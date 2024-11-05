using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class TT_StaffnStudentReportDTO
    {
        public long ASMCL_Id { get; set; }
        public long MI_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public bool ASMCL_ActiveFlag { get; set; }


        public long ISMS_Id { get; set; }
        //public long MI_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public bool ISMS_ActiveFlag { get; set; }


        public Array year { get; set; }
        public Array clsdrp { get; set; }
        public Array subdrp { get; set; }
        public Array periodslst { get; set; }
        public Array gridweeks { get; set; }
        public Array categorylist { get; set; }
        public Array staffDrpDwn { get; set; }
        public string staffNamelst { get; set; }
        public Array secdrp { get; set; }

        public Array view { get; set; }

        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public DateTime? ASMAY_From_Date { get; set; }
        public DateTime? ASMAY_To_Date { get; set; }
        public DateTime? ASMAY_PreAdm_F_Date { get; set; }
        public DateTime? ASMAY_PreAdm_T_Date { get; set; }
        public int ASMAY_Order { get; set; }
        public int ASMAY_ActiveFlag { get; set; }

        public DateTime? ASMAY_Cut_Of_Date { get; set; }
        public int ASMAY_Pre_ActiveFlag { get; set; }
        public bool Is_Active { get; set; }

        public TT_SubjectwiseDTO[] Time_Table { get; set; }
        public TT_SubjectwiseDTO[] classarray { get; set; }
        public TT_SubjectwiseDTO[] subarray { get; set; }

        public TT_SubjectwiseDTO[] TT { get; set; }

        public long TTMC_Id { get; set; }
        public long TTFGD_Id { get; set; }
        public long TTFG_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long HRME_Id { get; set; }
        public long TTMD_Id { get; set; }
        public int TTMP_Id { get; set; }
        public string TTMD_DayName { get; set; }
        public string TTMP_PeriodName { get; set; }
        public string staffName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string TTMC_CategoryName { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long staf_from { get; set; }
        public long staf_to { get; set; }
        public string returnduplicatestatus { get; set;}

        public bool returnval;
        public string TTMSAB_Abbreviation { get; set; }
        public string Status { get; set; }
        public Array lista { get; set; }
    }
}
