using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class AdmCollegeMasterBatchDTO
    {
        public long ACMB_Id { get; set; }
        public long MI_Id { get; set; }
        public string ACMSN_SessionName { get; set; }
        public int ACMNS_Order { get; set; }
        public bool ACMSN_ActiveFlag { get; set; }
        public string message { get; set; }
        public Array batchlist { get; set; }
       public bool returnval { get; set; }
    }
}
