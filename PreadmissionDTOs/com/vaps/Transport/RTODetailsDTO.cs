using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class RTODetailsDTO
    {
        public long TRRTO_Id { get; set; }
        public long TRMV_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime? TRRTO_Insurance_FromDate { get; set; }
        public DateTime? TRRTO_Insurance_Todate { get; set; }
        public string TRRTO_Company_Name { get; set; }
        public DateTime? TRRTO_Tax_FromDate { get; set; }
        public DateTime? TRRTO_Tax_ToDate { get; set; }

        public DateTime? TRRTO_FC_FromDate { get; set; }
        public DateTime? TRRTO_FC_ToDate { get; set; }
        public DateTime? TRRTO_Permit_FromDate { get; set; }
        public DateTime? TRRTO_Permit_ToDate { get; set; }

        public DateTime? TRDC_Date { get; set; }
        public DateTime? TRRTO_Emission_FromDate { get; set; }
        public DateTime? TRRTO_Emission_ToDate { get; set; }
        public DateTime? TRRTO_Ceasefire_FromDate { get; set; }
        public DateTime? TRRTO_Ceasefire_ToDate { get; set; }
        public DateTime? TRRTO_GPS_GPRS_Fitted_date { get; set; }
        public Array fillvahicleno { get; set; }
        public Array getloaddata { get; set; }
        public string message { get; set; }
        public bool returnval { get; set; }
        public string TRMV_VehicleNo { get; set; }

        public Array geteditdata { get; set; }
        
    }
}
