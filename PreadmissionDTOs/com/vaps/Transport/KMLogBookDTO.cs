using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class KMLogBookDTO
    {
        public long TRKMLB_Id { get; set; }
        public long MI_Id { get; set; }
        public long TRMV_Id { get; set; }
        public DateTime TRKMLB_EntryDate { get; set; }
        public DateTime TRKMLB_FromDate { get; set; }
        public string TRKMLB_FromTime { get; set; }
        public DateTime TRKMLB_ToDate { get; set; }
        public string TRKMLB_ToTime { get; set; }
        public string TRKMLB_OpeningReading { get; set; }
        public string TRKMLB_ClosingReading { get; set; }
        public long TRKMLB_NoOfKM { get; set; }
        public long TRMVT_Id { get; set; }
        public string TRKMLB_Remarks { get; set; }
        public bool TRKMLB_ActiveFlag { get; set; }
        public Array getloaddata { get; set; }
        public Array geteditdata { get; set; }
        public string message { get; set; }
        public bool returnval { get; set; }
        public Array filldrivrname { get; set; }
        public Array fillvehicletype { get; set; }
        public Array fillvahicleno { get; set; }
        public Array servicestlist { get; set; }
        public Array monthlist { get; set; }
        public string TRMD_DriverName { get; set; }
        public string TRMV_VehicleNo { get; set; }
        public vehicleid[] vhlid { get; set; }

        public Array fillkmreport { get; set; }
    }
    
}
