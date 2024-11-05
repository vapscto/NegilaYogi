using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class SMSParameters : CommonParamDTO
    {
        public long mobilenumber { get; set; }
        public string messagepart { get; set; }
        public DateTime Donetime { get; set; }
        public int status { get; set; } //create on 25Jan2016
    }
}
