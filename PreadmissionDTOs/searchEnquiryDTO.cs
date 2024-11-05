using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class searchEnquiryDTO : CommonParamDTO
    {
        public int id { get; set; }
        public string enquiryChoice { get; set; }

        public long MI_ID { get; set; }
        public long ASMAY_Id { get; set; }
        public long userid { get; set; }


    }
}
