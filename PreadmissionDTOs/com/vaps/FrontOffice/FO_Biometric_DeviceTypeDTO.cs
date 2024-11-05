using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.FrontOffice
{
    public class FO_Biometric_DeviceTypeDTO
    {
        public long FOBDT_Id { get; set; }
        public string FOBDT_DeviceType { get; set; }
        public bool FOBDT_ActiveFlg { get; set; }
        public string FOBDT_CodeRef { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
