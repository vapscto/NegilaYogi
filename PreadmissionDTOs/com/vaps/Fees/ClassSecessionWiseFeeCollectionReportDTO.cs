using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class ClassSecessionWiseFeeCollectionReportDTO
    {
        public long MI_Id { get; set; }
        public Array userlist { get; set; }
        public string NormalizedUserName { get; set; }
        public int userId { get; set; }
        public Array acayear { get; set; }
        public Array groupslist { get; set; }
      

        public Array reportdatelist { get; set; }
    }
  
}
