using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class SMS_MAIL_PARAMETER_DTO : CommonParamDTO
    {
        public long ISMP_ID { get; set; }
        public string ISMP_NAME { get; set; }

        public Array parameterList { get; set; }


    }
}
