using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class exammasterRemarkDTO
    {
        public int EPCR_Id { get; set; }
        public long MI_Id { get; set; }
        public string EPCR_RemarksName { get; set; }
        public int EPCR_RemarksOrder { get; set; }
        public bool EPCR_ActiveFlag { get; set; }
        public Array exammasterRemaksname { get; set; }
        public Array exammRemakname { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public string retrunMsg { get; set; }
        public bool already_cnt { get; set; }
        public exammasterRemarkDTO[] examRemarkDTO { get; set; }
        
        //Exam Wise Student Remarks
        public string flag { get; set; }
        public int roleid { get; set; }
        public string rolename { get; set; }
        public long User_Id { get; set; }
        public long Emp_Code { get; set; }
        public string UserName { get; set; }
        public Array classlist { get; set; }
        public Array sectionlist { get; set; }
        public Array yearlist { get; set; }
        public Array loaddata { get; set; }
        public Array personlaitylist { get; set; }
        public Array studentList { get; set; }
        public Array savedata { get; set; }
        public Array monthlist { get; set; }
        public Array examlist { get; set; }
        public Array remarkslist { get; set; }
        public long ASMCL_Id { get; set; }
        public int EME_Id { get; set; }
        public long ISMS_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ISMS_SubjectName { get; set; }
        public long ASMS_Id { get; set; }
        public string ASMC_SectionName { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public int ASMC_Order { get; set; }
        public int ASMAY_Order { get; set; }
        public int ASMCL_ClassOrder { get; set; }
        public Array configuration { get; set; }
        public string studentname { get; set; }
        public long AMST_Id { get; set; }
        public string AMST_AdmNo { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public long AMAY_RollNo { get; set; }
        public string EME_ExamName { get; set; }
        public int EME_ExamOrder { get; set; }
        public string EMER_Remarks { get; set; }
        public Temp_studentdata_Remarks[] Temp_studentdata_Remarks { get; set; }
        public Array Staffmobileappprivileges { get; set; }
        public string Pagename { get; set; }
        public string Pageicon { get; set; }
        public string Pageurl { get; set; }
        public long? IVRMRMAP_Id { get; set; }
        public bool? IVRMMAP_AddFlg { get; set; }
        public bool? IVRMMAP_UpdateFlg { get; set; }
        public bool? IVRMMAP_DeleteFlg { get; set; }
        public string mobileprivileges { get; set; }
        public string stringmobileorportal { get; set; }
        public string ESPCSR_Remarks { get; set; }
        public long Userid { get; set; }
        public DateTime? createddate { get; set; }
        public Array getstudentmarks { get; set; }
        public Array subjectlist { get; set; }
        public Array get_subjectwiseremarks { get; set; }
        public Array get_viewsubjectwiseremarks { get; set; }
        public long ISMS_OrderFlag { get; set; }
        public int ESPCSR_Id { get; set; }
    }
    public class Temp_studentdata_Remarks
    {
        public long AMST_Id { get; set; }
        public string EMER_Remarks { get; set; }
        public int ESSEPCR_Id { get; set; }
        public int ESPCR_Id { get; set; }
    }
}
