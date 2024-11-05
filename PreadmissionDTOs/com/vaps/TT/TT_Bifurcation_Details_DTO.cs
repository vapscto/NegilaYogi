using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class TT_Bifurcation_Details_DTO
    {
        public long TTBD_Id { get; set; }
        public long TTB_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long HRME_Id { get; set; }

        public long ISMS_Id { get; set; }
        public bool TTBD_ActiveFlag { get; set; }
    }
}
