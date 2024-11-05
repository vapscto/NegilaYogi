using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class StudentTrnspDTO : CommonParamDTO
    {
        public long PASRT_Id { get; set; }
        public long PASR_Id { get; set; }
        public long PASRT_ivrmmcT_Id { get; set; }
        public long PASRT_cmR_Id { get; set; }
        public long PASRT_cmL_Id { get; set; }
        public long PASRT_consession_type_Id { get; set; }
        public int PASRT_Daughter { get; set; }
        public int PASRT_Son { get; set; }
        public int PASRT_Heared_Friend_Colleague { get; set; }
        public int PASRT_Internet { get; set; }
        public int PASRT_Media { get; set; }
        public int PASRT_Other { get; set; }
    }
}
