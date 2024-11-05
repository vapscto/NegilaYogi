using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class SMS_MAIL_SAVED_PARAMETER_DTO : CommonParamDTO
    {
        public long ISMSPP_ID { get; set; }
        public long ISMP_ID { get; set; }
        public long MI_Id { get; set; }
        public long ISES_Id { get; set; }
        public string Flag { get; set; }
    }
}
