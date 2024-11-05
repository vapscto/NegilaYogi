using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeECSDTO
    {
        public string Admno { get; set; }
        public string PayeeName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Transactionid { get; set; }
        public long Amount { get; set; }
        public long Fine { get; set; }
        public string Transdate { get; set; }
        
    }
}
