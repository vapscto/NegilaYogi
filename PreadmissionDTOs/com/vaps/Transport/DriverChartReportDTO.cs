using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class DriverChartReportDTO
    {
        public long TRDC_Id { get; set; }
        public long MI_Id { get; set; }
        public decimal TRDC_FromKM { get; set; }
        public decimal TRDC_ToKM { get; set; }
        public decimal TRDC_RateKm { get; set; }
        public DateTime? TRDC_Date { get; set; }
        public long TRMV_Id { get; set; }
        public long TRMD_Id { get; set; }
        public string TRDC_Indent_BillNo { get; set; }
        public decimal TRDC_NoofLtr { get; set; }
        public decimal TRDC_TotalKM { get; set; }
        public decimal TRDC_TotalMileage { get; set; }
        public decimal TRDC_TotalAmount { get; set; }
        public decimal TRDC_AddtAmt { get; set; }
        public decimal TRDC_Emission { get; set; }
        public string TRDC_Remarks { get; set; }
        public decimal TRDC_GrossAmount { get; set; }
        public Array getloaddata { get; set; }
        public Array geteditdata { get; set; }
        public string message { get; set; }
        public bool returnval { get; set; }
        public Array filldrivrname { get; set; }
        public Array fillvahicleno { get; set; }
        public string TRMD_DriverName { get; set; }
        public string TRMV_VehicleNo { get; set; }
        public Array fillvahicletype { get; set; }
        public DateTime TODATE { get; set; }
        public DateTime FRMDATE { get; set; }
        public long TRMVT_Id { get; set; }
        public string TRMFT_FuelType { get; set; }

public vehicleid[] vhlid { get; set; }
    }

    public class vehicleid
    {
        public long TRMV_Id { get; set; }
    }
}
