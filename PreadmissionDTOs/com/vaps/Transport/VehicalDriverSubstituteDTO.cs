using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class VehicalDriverSubstituteDTO
    {
        public long TRVDST_Id { get; set; }
        public long MI_Id { get; set; }
   
        public DateTime TRVDS_FromDate { get; set; }
        public DateTime TRVDS_ToDate { get; set; }
        public long TRMV_Id { get; set; }
        public long TRVDS_AbsentDriverId { get; set; }

        public long TRVDS_SubstituteDriverId { get; set; }

        public Array driverdata { get; set; }
        public Array vehicaldata { get; set; }
        public Array getdata { get; set; }
        public Array savedata { get; set; }
         public Array vehicaldriverdata { get; set; }
        public Array editdata { get; set; }

        public string TRMD_DriverName { get; set; }
        public string TRMV_VehicleName { get; set; }
        public string message { get; set; }
        public bool retrunval { get; set; }
        public string Absent_Driver { get; set; }
        public string Substitute_Driver { get; set; }
    }
}
