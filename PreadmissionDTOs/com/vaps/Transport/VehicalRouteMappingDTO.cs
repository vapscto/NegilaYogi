using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class VehicalRouteMappingDTO
    {

        public long TRML_Id { get; set; }
        public long MI_Id { get; set; }
        public long TRVR_Id { get; set; }
        public long TRMV_Id { get; set; }
        public long TRMR_Id { get; set; }
         public long TRMS_Id { get; set; }
         public bool TRVR_ActiveFlg { get; set; }

        public DateTime TRVR_Date { get; set; }

        public Array routedata { get; set; }
        public Array sessiondata { get; set; }
        public Array vehicaldata { get; set; }
        public string TRMV_VehicleName { get; set; }
        public string TRMV_VehicleNo { get; set; }
        public string TRMR_RouteName { get; set; }
        public string TRMS_SessionName { get; set; }
        public string TRMS_Flag { get; set; }
        public Array dispdata { get; set; }
        public Array getdata { get; set; }
        public Array savedata { get; set; }

        public Array editdata { get; set; }
     //   public Array child_edit_data { get; set; }


        public string message { get; set; }
        public bool retrunval { get; set; }
    }
}
