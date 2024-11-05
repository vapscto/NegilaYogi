using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class tempPrivilagesDTO
    {
       public long yearid { get; set; }
        public int user_id { get; set; }
        public string year { get; set; }
        public string emp_name { get; set; }
        public string elpflg { get; set; }
        public tempPrivilagesDTO1 clas { get; set; }
        public tempPrivilagesDTO2 secs { get; set; }
        public tempPrivilagesDTO3[] sub { get; set; }
        public tempPrivilagesDTO4[] ssub { get; set; }
    }
    public class tempPrivilagesDTO1
    {
        //  public long asmcL_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public int ASMCL_Order { get; set; }
        public long MI_Id { get; set; }

        public int ASMCL_MinAgeYear { get; set; }
        public int ASMCL_MinAgeMonth { get; set; }
        public int ASMCL_MinAgeDays { get; set; }
        public int ASMCL_MaxAgeYear { get; set; }
        public int ASMCL_MaxAgeMonth { get; set; }
        public int ASMCL_MaxAgeDays { get; set; }
        public int ASMCL_MaxCapacity { get; set; }
        public bool ASMCL_ActiveFlag { get; set; }

    }
    public class tempPrivilagesDTO2
    {
        //   public long asmS_Id { get; set; }
        public long ASMS_Id { get; set; }

        public long MI_Id { get; set; }
        public string ASMC_SectionName { get; set; }
        public string ASMC_SectionCode { get; set; }
        public int ASMC_Order { get; set; }
        public int ASMC_ActiveFlag { get; set; }
        public int ASMC_MaxCapacity { get; set; }

    }
    public class tempPrivilagesDTO3
    {
        //   public long ismS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long MI_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public decimal? ISMS_Max_Marks { get; set; }
        public decimal? ISMS_Min_Marks { get; set; }
        public long ISMS_ExamFlag { get; set; }
        public long ISMS_PreadmFlag { get; set; }
        public long ISMS_SubjectFlag { get; set; }
        public long ISMS_BatchAppl { get; set; }
        public long ISMS_ActiveFlag { get; set; }
        public long ISMS_OrderFlag { get; set; }
        public bool ISMS_TTFlag { get; set; }
        public bool ISMS_AttendanceFlag { get; set; }

    }
    public class tempPrivilagesDTO4
    {
        //  public long emsS_Id { get; set; }
        public int EMSS_Id { get; set; }
        public long MI_Id { get; set; }
        public string EMSS_SubSubjectName { get; set; }
        public string EMSS_SubSubjectCode { get; set; }
        public int EMSS_Order { get; set; }
        public bool EMSS_ActiveFlag { get; set; }
    }
}
