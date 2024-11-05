using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class CollegeStudentTCReportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public string allorindi { get; set; }
        public Array getyear { get; set; }
        public Array getcourse { get; set; }
        public Array getbranch { get; set; }
        public Array getsemester { get; set; }
        public Array getsection { get; set; }
        public Array getreport { get; set; }
        public CollegeStudentTCReportDTO[] TempararyArrayheadList { get; set; }
        public string tcperortemp { get; set; }
        public string columnName { get; set; }
        public string columnID { get; set; }

        // TC Custom Report
        public long AMCST_Id { get; set; }
        public string admnoorname { get; set; }
        public string tctemporper { get; set; }
        public string companyname { get; set; }
        public Array getstudent { get; set; }
        public Array getstudentdetails { get; set; }
        public Array academicList1 { get; set; }
        public Array MasterCompany { get; set; }
    }
}
