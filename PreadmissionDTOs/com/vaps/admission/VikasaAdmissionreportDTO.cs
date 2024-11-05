 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class VikasaAdmissionreportDTO
    {
        public string stuname;
        public Array previousyear { get; set; }
        public long userid { get; set; }
        public long AMST_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMC_Id { get; set; }
        public long? ASMCL_Id { get; set; }
        public Array AllAcademicYear { get; set; }
        public Array AllClass { get; set; }
        public Array AllSection { get; set; }
        public long asms_id { get; set; }
        public long empid { get; set; }
        public Array fillstudlist { get; set; }
        public Array studentlist { get; set; }
        public Array adm_m_student { get; set; }
        public Array employeedetails { get; set; }
        public string AMST_FirstName { get; set; }
        public string empname { get; set; }
        public string AMST_SOL { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public string AMST_AdmNo { get; set; }
        public DateTime AMST_DOB { get; set; }
        public string AMST_Sex { get; set; }
        public string AMST_DOB_Words { get; set; }
        public string AMST_FatherName { get; set; }
        public string searchfilter { get; set; }
        public string classname { get; set; }
        public string sectionname { get; set; }
        public string radiobutton { get; set; }
        public string paidamount { get; set; }
        public DateTime doa { get; set; }
        public string save_flag { get; set; }
        public string message { get; set; }
        public long count { get; set; }
        public Array mastercompany { get; set; }
        public long mobileno { get; set; }
        public string address { get; set; }
        public string deptname { get; set; }
    }
}
