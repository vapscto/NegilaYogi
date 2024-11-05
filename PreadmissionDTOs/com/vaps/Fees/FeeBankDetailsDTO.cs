using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeBankDetailsDTO
    {
        public int FBD_ID { get; set; }
        public string Class_Category { get; set; }
        public string Class { get; set; }
        public string Bank_Name { get; set; }
        public string Bank_Address { get; set; }
        public string Acc_No { get; set; }
        public int L_code { get; set; }
        public string IFSC { get; set; }
        public string ACC_name { get; set; }
    }
}
