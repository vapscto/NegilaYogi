using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class TransportReportDTO
    {
        public object messagelist { get; set; }
        public long TRMA_Id { get; set; }
        public long MI_Id { get; set; }
        public int TRMR_order { get; set; }
        public string TRMA_AreaName { get; set; }
        public string TRMA_AliasName { get; set; }
        public string onclickloaddata { get; set; }
        public string regorname_m { get; set; }
        public object TRRSC_Id { get; set; }
        public object TRRSC_ScheduleName { get; set; }
        public object TRRSC_Date { get; set; }
        public object TRMR_RouteName { get; set; }
        public object TRRSC_ActiveFlag { get; set; }
        public string regorname_map { get; set; }
        public long TRMRL_Id { get; set; }
        public string routename { get; set; }
        public string TRMR_RouteNo { get; set; }
        public bool TRVD_ActiveFlg { get; set; }
        public string TRMS_SessionName { get; set; }
        public string TRMS_Flag { get; set; }
        public string locationname { get; set; }
        public long TRML_Id { get; set; }
        public long TRMR_Id { get; set; }
        public bool TRMRL_ActiveFlag { get; set; }
        public long TRVR_Id { get; set; }
        public long TRVD_Id { get; set; }
        public string TRMV_VehicleName { get; set; }
        public string TRMD_DriverName { get; set; }
        public DateTime TRVD_Date { get; set; }
        public long TRMS_Id { get; set; }
        public DateTime TRVR_Date { get; set; }
        public bool TRVR_ActiveFlg { get; set; }
    }
}
