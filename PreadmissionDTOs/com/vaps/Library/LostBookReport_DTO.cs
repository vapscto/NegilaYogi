using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Library
{
   public class LostBookReport_DTO:CommonParamDTO
    {

        public string BookType { get; set; }
        public long MI_Id { get; set; }
        public string BNBFlg { get; set; }
        public string Fromdate { get; set; }
        public string ToDate { get; set; }
        public Array reportlist { get; set; }
        public Array lib_list { get; set; }
        public long LMAL_Id { get; set; }
        public long IVRMUL_Id { get; set; }

    }
}
