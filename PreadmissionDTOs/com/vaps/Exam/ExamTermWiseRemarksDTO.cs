using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class ExamTermWiseRemarksDTO
    {
        public long ECTERE_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public int ECT_Id { get; set; }
        public long AMST_Id { get; set; }
        public string ECTERE_Remarks { get; set; }
        public bool ECTERE_ActiveFlag { get; set; }
        public string ECTERE_Indi_OverAllFlag { get; set; }
        public string indiorfinal { get; set; }
        public string flag { get; set; }
        public int roleid { get; set; }
        public string rolename { get; set; }
        public long User_Id { get; set; }
        public long Emp_Code { get; set; }
        public string UserName { get; set; }
        public Array getyear { get; set; }
        public Array getclass { get; set; }
        public Array getsection { get; set; }
        public Array getterm { get; set; }
        public Array getstudentdetails { get; set; }
        public Array getsavedetails { get; set; }
        public Array getdetails { get; set; }
        public Array geteditdetails { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMS_SectionName { get; set; }
        public string ASMAY_Year { get; set; }
        public string ECT_TermName { get; set; }
        public string studentname { get; set; }
        public string admno { get; set; }
        public string regno { get; set; }
        public string message { get; set; }
        public long rollno { get; set; }       
        public bool returnval { get; set; }       
        public saved_remarks_details[] saved_remarks_details { get; set; }
        public saved_participate_details[] saved_participate_details { get; set; }
        public Array Staffmobileappprivileges { get; set; }
        public Array viewstudentdetails { get; set; }
        public string Pagename { get; set; }
        public string Pageicon { get; set; }
        public string Pageurl { get; set; }
        public long? IVRMRMAP_Id { get; set; }
        public bool? IVRMMAP_AddFlg { get; set; }
        public bool? IVRMMAP_UpdateFlg { get; set; }
        public bool? IVRMMAP_DeleteFlg { get; set; }
        public string mobileprivileges { get; set; }
        public string stringmobileorportal { get; set; }        
        public long Userid { get; set; }
        public string ESTTA_Remarks { get; set; }
        public string ECTERE_Conduct { get; set; }
        public bool ESTTA_ActiveFlag { get; set; }
        public long ESTTA_Id { get; set; }
    }
    public class saved_remarks_details
    {
        public long AMST_Id { get; set; }
        public long ECTERE_Id { get; set; }
        public string ECTERE_Remarks { get; set; }      
        public string studentname { get; set; }
        public string ECTERE_Conduct { get; set; }
    }
    public class saved_participate_details
    {
        public long AMST_Id { get; set; }
        public long ESTTA_Id { get; set; }
        public string ESTTA_Remarks { get; set; }      
        public string studentname { get; set; }
    }
}
