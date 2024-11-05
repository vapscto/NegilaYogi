using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.TT
{
   public class TT_ClasswiseConsolidatedReportDTO
    {

        public long MI_Id { get; set; }
        public Array classlist { get; set; }
        public Array sectionlist { get; set; }
        public string tsallorindii { get; set; }
        
        public long ASMCL_Id { get; set; }
        public DateTime N_ActivityDate { get; set; }
        public long ASMS_Id { get; set; }
        public DateTime NC_ActivityDate { get; set; }
        public Array yearlist { get; set; }
        public long ASMAY_Id { get; set; }
      
       
        public Array getreportdata { get; set; }
        public Array subject { get; set; }
        public Array day { get; set; }
    }
}
