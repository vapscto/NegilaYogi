using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeHeadWiseReportDTO
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
        public bool Active_Flag { get; set; }
        public string  Fine_Applicable{ get; set; }
        public string Installment { get; set; }

        public decimal Fine_Amount { get; set; }
       
        public long user_id { get; set; }

        public long fillclasflg { get; set; }
        public long fillseccls { get; set; }

        public Array getreportdata { get; set; }
    }
}
