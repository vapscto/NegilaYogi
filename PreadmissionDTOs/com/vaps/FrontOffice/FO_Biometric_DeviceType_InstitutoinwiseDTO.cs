using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.FrontOffice
{
    public class FO_Biometric_DeviceType_InstitutoinwiseDTO
    {
        public long FOBDTI_Id { get; set; }
        public long MI_Id { get; set; }
        public long FOBDT_Id { get; set; }
        public bool FOBDTI_ActiveFlg { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
