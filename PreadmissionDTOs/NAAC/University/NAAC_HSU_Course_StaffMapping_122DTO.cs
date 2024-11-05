using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.University
{
   public class NAAC_HSU_Course_StaffMapping_122DTO
    {
        public long NCHSUSM122_Id { get; set; }
        public long ACAYC_Id { get; set; }
        public long HRME_Id { get; set; }
        public long NCHSUSM122_CreatedBy { get; set; }
        public long NCHSUSM122_UpdatedBy { get; set; }
        public bool NCHSUSM122_ActiveFlag { get; set; }
        public DateTime? NCHSUSM122_CreatedDate { get; set; }
        public DateTime? NCHSUSM122_UpdatedDate { get; set; }
        public int AMCO_Order { get; set; }
        public long UserId { get; set; }
        public long MI_Id { get; set; }
        public Array allacademicyear { get; set; }
        public Array alldata1 { get; set; }
        public Array institutionlist { get; set; }
        public string ASMAY_Year { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMCO_CourseCode { get; set; }
        public string empname { get; set; }
        public long asmaY_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long HRMDES_Id { get; set; }
        public bool duplicate { get; set; }
        public NAAC_HSU_Course_StaffMapping_122DTO[] filelist { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public string msg { get; set; }
        public bool returnval { get; set; }
        public Array editlist { get; set; }
        public Array editFileslist { get; set; }
        public Array viewuploadflies { get; set; }
        public Array departmentlist { get; set; }
        public Array courselist { get; set; }
        public Array designationlist { get; set; }
        public Array employeelist { get; set; }
        public long NCHSUSM122F_Id { get; set; }
        public long AMCO_Id { get; set; }
        public Array editYear { get; set; }
    }
}
