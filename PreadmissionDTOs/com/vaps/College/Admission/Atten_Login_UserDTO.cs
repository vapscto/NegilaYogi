using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class Atten_Login_UserDTO
    {
        public long ACALU_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ACALD_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public bool ACALD_ActiveFlag { get; set; }
        public Array yearlist { get; set; }
        public Array stafflist { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }
        public Array semisterlist { get; set; }
        public Array sectionlist { get; set; }
        public Array subjectlist { get; set; }
        public Array saveddata { get; set; }
        public bool returnval { get; set; }
        public bool returnduplicatestatus { get; set; }      
        public string HRME_EmployeeFirstName { get; set; }
        public Temp_login_detailsDTO[] Details { get; set; }
        public string ASMAY_Year { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMSE_SEMName { get; set; }
        public string ACMS_SectionName { get; set; }
        public string ISMS_SubjectName { get; set; }
    }   
    public class Temp_login_detailsDTO
    {
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long[] ISMS_Ids { get; set; }
        //public long ISMS_Id { get; set; }
    }
}
