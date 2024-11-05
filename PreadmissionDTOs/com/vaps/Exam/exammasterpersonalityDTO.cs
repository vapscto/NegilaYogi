using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class exammasterpersonalityDTO
    {
        public int EP_Id { get; set; }
        public string EP_PersonlaityName { get; set; }
        public string EP_PersonlaityCode { get; set; }
        public int EP_PersonlaityOrder { get; set; }
        public bool EP_ActiveFlag { get; set; }
        public long MI_Id { get; set; }
        public Array exammasterpersonalityname { get; set; }
        public Array exammpersonalityname { get; set; }
        public Array personalityorder { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public string retrunMsg { get; set; }
        public bool already_cnt { get; set; }
        public exammasterpersonalityDTO[] examPersonlityDTO { get; set; }


        // Student Personality mapping

        public long ASMCL_Id { get; set; }
        public int EME_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public long ASMS_Id { get; set; }
        public string ASMC_SectionName { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }
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
        public int Monthid { get; set; }
        public string monthname { get; set; }
        public Temp_studentdata[] Temp_studentdata { get; set; }
        public string flag { get; set; }
        public int roleid { get; set; }
        public string rolename { get; set; }
        public long User_Id { get; set; }
        public string UserName { get; set; }
        public int ASMC_Order { get; set; }
        public int ASMCL_ClassOrder { get; set; }
        public long Emp_Code { get; set; }
        public string studentname { get; set; }
        public long AMST_Id { get; set; }
        public string AMST_AdmNo { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public long AMAY_RollNo { get; set; }
        public Array configuration { get; set; }
        public string ESPM_Remarks { get; set; }       

    }

    public class Temp_studentdata
    {
        public long AMST_Id { get; set; }
        public string ESP_Remarks { get; set; }
    }
}
