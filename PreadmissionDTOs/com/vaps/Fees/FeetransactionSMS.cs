using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeetransactionSMS
    {
        public long MI_ID { get; set; }
        public string AMST_AdmNo { get; set; }
        public long? AMST_MobileNo { get; set; }
        public long? AMCST_MobileNo { get; set; }
        public string AMCST_emailId { get; set; }

        public string AMST_FatherName { get; set; }
        public string termname { get; set; }     
        public long userid { get; set; }
        public long FMT_ID { get; set; }
        public string msg { get; set; }
        public string StudentName { get; set; }
        public bool stdselected { get; set; }
        public string totalbalance { get; set; }
        public string paid { get; set; }
        public string ClassSection { get; set; }
        public FeetransactionSMS[] selectedStudentList { get; set; }
        public long AMST_ID  { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long[] FMG_Ids { get; set; }
        public long[] FMT_Ids { get; set; }
        public string term_group { get; set; }
        public string year { get; set; }
        public string month { get; set; }
        public string duration { get; set; }
        public string date { get; set; }
        public string Due_Date { get; set; }
        public List<FeetransactionSMS> Due_Date_array { get; set; }
        public Array feesummlist { get; set; }

    }
}
