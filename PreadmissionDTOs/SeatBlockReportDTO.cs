using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class SeatBlockReportDTO : CommonParamDTO
    {
        public Array fillyear { get; set; }
        public Array fillclass { get; set; }

        public string stdorregnoflag { get; set; }

        public Array studentlist { get; set; }

        public string PASR_FirstName { get; set; }
        public string PASR_MiddleName { get; set; }
        public string PASR_LastName { get; set; }
       
        public long pasr_id { get; set; }

        public string PASR_RegistrationNo { get; set; }

        public string asmayid { get; set; }
        public long asmclid { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public string flagows { get; set; }
        public Array allreports { get; set; }

        public int mid { get; set; }

        public string regornamedetails { get; set; }

        public long ASMAY_Id { get; set; }

        public long ASMCL_Id { get; set; }

        public long AMST_Id { get; set; }


        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
    }
}
