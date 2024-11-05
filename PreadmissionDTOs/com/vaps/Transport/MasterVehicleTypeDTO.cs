using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class MasterVehicleTypeDTO
    {
        public long TRMVT_Id { get; set; }
        public long MI_Id { get; set; }
        public string TRMVT_VehicleType { get; set; }

        public string TRMVT_VehicleDesc { get; set; }

        public bool TRMVT_ActiveFlg { get; set; }
        public string message { get; set; }
        public bool retrval { get; set; }
        public Array getmastervehicle { get; set; }
        public Array geteditdatavehicle { get; set; }
    }
}
