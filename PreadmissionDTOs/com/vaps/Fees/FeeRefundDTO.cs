using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeRefundDTO
    {
        public Array admsudentslist { get; set; }
        public Array academicyr { get; set; }
        public Array reportdatelist { get; set; }
        public Array studentlist { get; set; }
        public Array fillfeehead { get; set; }
        public Array fillclass { get; set; }
        public Array fillsec { get; set; }

        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public long Amst_Id { get; set; }
        public DateTime datedisplay { get; set; }
        public long MI_ID { get; set; }
        public long asmyid { get; set; }
        public long ASMCL_Id { get; set; }
        public string filterinitialdata { get; set; }
        public string regornamedetails { get; set; }


        //added by kiran
        public long userid { get; set; }
        
        public long AMSC_Id { get; set; }
        public string asmc_sectionname { get; set; }
        public Array refunddata { get; set; }

        public long fmh_id  {get;set;}

        public string fmh_feename { get; set; }
        public DateTime? Fromdate { get; set; }
        public DateTime? Todate { get; set; }
    }
}
