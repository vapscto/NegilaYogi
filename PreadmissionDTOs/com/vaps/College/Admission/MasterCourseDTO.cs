using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class MasterCourseDTO
    {
        public long AMCO_Id { get; set; }
        public long MI_Id { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMCO_CourseCode { get; set; }
        public string AMCO_CourseInfo { get; set; }
        public bool AMCO_CourseFlag { get; set; }
        public int AMCO_NoOfYears { get; set; }
        public int AMCO_NoOfSemesters { get; set; }
        public double AMCO_MinAttPer { get; set; }
        public bool AMCO_FeeAplFlg { get; set; }
        public int AMCO_Order { get; set; }
        public bool AMCO_RegFeeFlg { get; set; }
        public bool AMCO_ActiveFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public  bool returnval { get; set;}
        public bool duplicateval { get; set; }
        public Array MasterCourseList { get; set; }
        public Array MasterCourseList1 { get; set; }
        public coursedto[] coursedto { get; set; }
        public string retrunMsg { get; set; }
        public Array editdetails { get; set; }
        public long ASMAY_Id { get; set; }
        public string msg { get; set; }
    }
    public class coursedto
    {
        public long AMCO_Id { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMCO_CourseCode { get; set; }
        public int AMCO_Order { get; set; }
    }
}
