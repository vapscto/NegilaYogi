using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class VehicalDriverMappingDTO
    {
        public long TRML_Id { get; set; }
        public long MI_Id { get; set; }
        public long TRVD_Id { get; set; }

        public DateTime  TRVD_Date { get; set; }
        public long TRMV_Id { get; set; }
        public string TRMS_Flag { get; set; }

        public long TRMS_Id { get; set; }
        public bool TRVD_ActiveFlg { get; set; }
        public bool TRMD_ActiveFlg { get; set; }

        public long TRMD_Id { get; set; }
        public Array driverdata { get; set; }
        public Array vehicaldata { get; set; }
        public Array sessiondata { get; set; }

        public Array getdata { get; set; }
        public Array savedata { get; set; }

        public Array editdata { get; set; }

        public string TRMD_DriverName { get; set; }
        public string TRMV_VehicleName { get; set; }
        public string TRMV_VehicleNo { get; set; }
        public  string TRMS_SessionName { get; set; }
        public string message { get; set; }
        public bool retrunval { get; set; }
    }
}
