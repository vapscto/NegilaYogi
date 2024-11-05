using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class subjectmasterDTO :CommonParamDTO
    {
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
        public string AMSU_Flag { get; set; }
        public long ISMS_OrderFlag { get; set; }
        //BatchFlag

       // public string BatchFlag { get; set; }
        public Array subjectmastername { get; set; }
        public bool returnval { get; set; }
        public int count { get; set; }
        public string msg { get; set; }

      //  public int? amsu_activeflag { get; set; }

     //   public string TimeTable_flag { get; set; }
       
    }
}
