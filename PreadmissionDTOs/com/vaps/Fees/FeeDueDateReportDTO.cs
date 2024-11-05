using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeDueDateReportDTO
    {
        public long ASMAY_Id { get; set; }

        public long MI_Id { get; set; }

        public long FMCC_Id { get; set; }

        public Array YearList { get; set; }
        public Array Class_Category_List { get; set; }

        public string FHWR_ClassCategoryName { get; set; }
        public Array FHWR_searchdatalist { get; set; }
        public string Fee_Group { get; set; }
        public string Fee_Head { get; set; }
        public string Master_Installment { get; set; }
        public string Tran_Installment { get; set; }
        public bool Active_Flag { get; set; }
        public DateTime? Due_Date { get; set; }
        public string Fine_Slab { get; set; }
        public string Fine_Type { get; set; }
        public decimal Fine_Amount { get; set; }

        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }
        public Array GroupHeadData { get; set; }
        public long user_id { get; set; }
        public DateTime? Fromdate { get; set; }
        public DateTime? Todate { get; set; }
        public Array incomereport { get; set; }
        public Array expensereport { get; set; }
        
    }
}
