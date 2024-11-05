using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Inventory
{
   public class INV_StockSummaryDTO
    {
        public long MI_Id { get; set; }
        public string optionflag { get; set; }
        public itemsArrayDTO[] itemsArray { get; set; }
        public DateTime? startdate { get; set; }
        public DateTime? enddate { get; set; }
        public Array get_report { get; set; }
        public Array stock_summaryreport { get; set; }
        public string logopath { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public Array Select_list { get; set; }
        public Array Section_list { get; set; }
        public Array getstudent { get; set; }
        public sectionlist[] sections { get; set; }
        public Array yearlist { get; set; }
        public long AMST_Id { get; set; }
        public string studentname { get; set; }
        public Array invstockreport { get; set; }
    }
    public class sectionlist
    {
        public long ASMS_Id { get; set; }
    }
}
