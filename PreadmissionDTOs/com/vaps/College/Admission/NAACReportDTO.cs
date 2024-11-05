using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class NAACReportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public string flag { get; set; }
        public Array acdlist { get; set; }
        public Array courselist { get; set; }
        public NAACReportDTO[] TempararyArrayListcoloumn { get; set; }
        public Array datareport { get; set; }
        public Array datareport1 { get; set; }
        public Array datareport2 { get; set; }
        public string ACQC_CategoryName { get; set; }
        public int No_of_Seats { get; set; }
        public long ACQC_Id { get; set; }
        public string AMCST_Sex { get; set; }
        public string ASMAY_Year { get; set; }
        public long ACQ_Id { get; set; }
        public string ACQ_QuotaName { get; set; }
        public string AMCOC_Name { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMCO_CourseName { get; set; }
        public int AMCO_NoOfYears { get; set; }
        public int Std_Approve { get; set; }
        public int No_of_Adm { get; set; }
        public int No_of_dept { get; set; }
        public long AMCOC_Id { get; set; }
        public long AMB_Id { get; set; }
        public string male { get; set; }
        public string female { get; set; }
        public Array religion { get; set; }
        public Array castecateogry { get; set; }
        public Array semester { get; set; }
        public int AMSE_Year { get; set; }

    }
}
