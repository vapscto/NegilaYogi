using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.admission
{
  public class JSHSAdmissionCertificate_DTO
    {

        public long MI_Id { get; set; }
        public long userId { get; set; }
        public string AMST_FirstName { get; set; }
        public long AMST_Id { get; set; }
        public string AMST_SOL { get; set; }
        public Array fillstudlist { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public string AMST_AdmNo { get; set; }
        public Array AllAcademicYear { get; set; }
        public Array adm_m_student { get; set; }
        public string IMC_CasteName { get; set; }
        public long ASMAY_Id { get; set; }
        public long? ASMCL_Id { get; set; }
        public long ASMC_Id { get; set; }
        public Array studentlist { get; set; }
        public Array academicYearOnLoad { get; set; }
        public Array StudentList { get; set; }
        public Array academicyearforreadmit { get; set; }
        public Array MasterCompany { get; set; }
        public string companyname { get; set; }
        public Array academicList1 { get; set; }
        public Array allsectionlist { get; set; }
        public Array allclasslist { get; set; }
        public string searchfilter { get; set; }
        public int count { get; set; }
        public string photopath { get; set; }
        public string allorindid { get; set; }
        public Array principalsign { get; set; }
        public DateTime dob { get; set; }
        public string dobwords { get; set; }
        public string fathername { get; set; }
        public DateTime joinedyear { get; set; }
        public DateTime leftyear { get; set; }
        public string joinedclass { get; set; }
        public string leftclass { get; set; }
        public string save_flag { get; set; }
        public string message { get; set; }
        public string allorindi { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string radiobutton { get; set; }
    }
}
