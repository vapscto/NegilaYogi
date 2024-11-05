using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Sports
{
   public class HouseInchargeReport_DTO:CommonParamDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public Array yearlist { get; set; }
        public Array houseList { get; set; }
        public Array report_list { get; set; }
        public long SPCCMH_Id { get; set; }


        public HouseInchargeReport_DTO[] selectedhouselist { get; set; }


    }
}
