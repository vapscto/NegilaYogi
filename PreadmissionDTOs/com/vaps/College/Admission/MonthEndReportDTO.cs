using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class MonthEndReportDTO
    {
        public long MI_ID { get; set; }
        public long AMCOC_Id { get; set; }
        public Array acayear { get; set; }
        public string monthpass { get; set; }
        public string acayid { get; set; }
        public DateTime? frmdate { get; set; }
        public DateTime? todate { get; set; }
        public Array reportdatelist { get; set; }
        public string cashcount { get; set; }
        public string onlinecount { get; set; }
        public string esccount { get; set; }
        public string newadmission { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public Array Month_array { get; set; }
        public Array getcategory { get; set; }

        public Array courselist { get; set; }

        public Array branchlist { get; set; }

        public Array semesterlist { get; set; }

        public Array studentlist { get; set; }

        public string AMCST_SOL { get; set; }

        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long AMCOBM_Id { get; set; }
        public string AMSE_SEMName { get; set; }

        public long AMB_Id { get; set; }

        public string AMB_BranchName { get; set; }
        public string Status_Flag { get; set; }

        public string withtc { get; set; }
        public string withdeactive { get; set; }

        public int totalgender { get; set; }
        public string gender1 { get; set; }
    }
}
