using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeCVMstudentDTO
    {
        public long CMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public long CTR_Id { get; set; }
        public DateTime CMS_Date { get; set; }
        public string CMS_Rate { get; set; }
        public long CTRB_Id { get; set; }
        public string CMS_Remarks { get; set; }
    

    }
}
