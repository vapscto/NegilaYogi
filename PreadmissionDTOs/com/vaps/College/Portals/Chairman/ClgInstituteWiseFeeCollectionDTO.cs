using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Portals.Chairman
{
    public class ClgInstituteWiseFeeCollectionDTO
    {
       
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public decimal ballance { get; set; }
        public decimal recived { get; set; }
        public decimal paid { get; set; }
        public decimal concession { get; set; }
        public decimal waived { get; set; }
        public decimal rebate { get; set; }
        public decimal fine { get; set; }
        public decimal receivable { get; set; }
        public long user_id { get; set; }

        public long[] mi_ids { get; set; }

        public long[] monthids { get; set; }
        public Array monthname { get; set; }

        public Array institutename { get; set; }

        public string MI_Name { get; set; }

        public long IVRM_Month_Id { get; set; }
        public string IVRM_Month_Name { get; set; }
        public Array institutedata { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string FYP_Date { get; set; }
    }
}
