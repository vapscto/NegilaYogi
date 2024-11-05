using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class StudentAddressBookDTO
    {
        public long MI_Id { get; set; }
        public Array academiclist { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }
        public Array alldata { get; set; }
        public Array reportdata { get; set; }
        public Array semlist { get; set; }
        public Array seclist { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMB_BranchName { get; set; }
        public Array studentlist { get; set; }
        public Temp_branchDTOreport[] Temp_branchDTOreport { get; set; }
        public string studentname { get; set; }
        public string AMCST_RegistrationNo { get; set; }
        public string AMCST_AdmNo { get; set; }
        public long AMCST_Id { get; set; }
        public Temp_StudentListDto[] Temp_StudentListDto { get; set; }
    }
    public class Temp_branchDTOreport
    {
        public long AMB_Id { get; set; }
    }
    public class Temp_StudentListDto
    {
        public long AMCST_Id { get; set; }
    }
}
