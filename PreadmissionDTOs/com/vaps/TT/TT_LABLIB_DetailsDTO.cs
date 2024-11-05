using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class TT_LABLIB_DetailsDTO
    {
        public long TTLABD_Id { get; set; }
        public long TTLAB_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ISMS_Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }


        public bool TTLABD_ActiveFlag { get; set; }



    }
}
