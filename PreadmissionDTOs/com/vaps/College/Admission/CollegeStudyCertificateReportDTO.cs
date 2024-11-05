using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class CollegeStudyCertificateReportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long AMCST_Id { get; set; }
        public Array getyear { get; set; }
        public Array getcourse { get; set; }
        public Array getbranch { get; set; }
        public Array getsemester { get; set; }
        public Array getsection { get; set; }
        public Array getstudentlist { get; set; }
        public Array getstudentdetailslist { get; set; }
        public Array academicList1 { get; set; }
        public string studentname { get; set; }
        public string searchfilter { get; set; }
        public string AMCST_SOL { get; set; }
        public string allorindid { get; set; }
        public string coursename { get; set; }
        public string branchname { get;set; }
        public string semestername { get; set; }
        public string sectionname { get; set; }
        public string AMCST_Sex { get; set; }

        public string registrationno { get; set; }
        public string yearname { get; set; }
        public string admno { get; set; }
        public string gender { get; set; }
        public int count { get; set; }
        
        public string type { get; set; }

        //
        public string fathername { get; set; }
        public string classstudying { get; set; }
        public string acadamicyear { get; set; }
        public string religion { get; set; }
        public string castecat { get; set; }
        public DateTime? dob { get; set; }

        public string logo { get; set; }
    }
}
