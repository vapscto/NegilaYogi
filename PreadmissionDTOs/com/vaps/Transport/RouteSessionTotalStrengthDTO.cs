using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class RouteSessionTotalStrengthDTO
    {
        public long TRSR_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMST_Id { get; set; }
        public DateTime TRSR_Date { get; set; }
        public long FMG_Id { get; set; }
        public long TRMR_Id { get; set; }
        public long TRSR_PickupSchedule { get; set; }
        public long TRSR_PickUpLocation { get; set; }
        public long TRSR_PickUpMobileNo { get; set; }
        public long TRSR_DropSchedule { get; set; }
        public long TRSR_DropLocation { get; set; }
        public long TRSR_DropMobileNo { get; set; }
        public long TRSR_ApplicationNo { get; set; }
        public bool TRSR_ActiveFlg { get; set; }
        public long TRRSC_Id { get; set; }
       
        public long TRMA_Id { get; set; }
        public string TRMR_RouteName { get; set; }
        public string TRMR_RouteNo { get; set; }
        public string TRMR_RouteDesc { get; set; }
        public bool TRMR_ActiveFlg { get; set; }


        public DateTime TRRSC_Date { get; set; }
        public string TRRSC_ScheduleName { get; set; }
        public bool TRRSC_ActiveFlag { get; set; }


        public Array YearList { get; set; }


        public long stud_count { get; set; }
        public long totaltrcount { get; set; }
        public Array messagelist { get; set; }
        public Array griddata { get; set; }
        public Array schedultime { get; set; }
        




    }
}
