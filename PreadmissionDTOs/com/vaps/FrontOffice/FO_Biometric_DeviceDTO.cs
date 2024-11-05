using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.FrontOffice
{
    public class FO_Biometric_DeviceDTO
    {
        public long FOBD_Id { get; set; }
        public string FOBD_DeviceName { get; set; }
        public string FOBD_IPAddress { get; set; }
        public string FOBD_DeviceType { get; set; }
        public long FOBD_DevicePortNo { get; set; }
        public string FOBD_DevicePassword { get; set; }
        public bool FOBD_ActiveFlg { get; set; }
        public string FOBD_StudStaffFlg { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
