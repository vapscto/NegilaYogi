using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class AdditionalFieldDTO:CommonParamDTO
    {
        public long IPAF_Id { get; set; }
        public long Page_Id { get; set; }
        public int IPAF_Flag { get; set; }
        public string IPAF_Name { get; set; }
        public string IPAF_Type { get; set; }
        public decimal IPAF_Size { get; set; }
        public decimal IPAF_Scale { get; set; }
        public int IPAF_Apl_Report { get; set; }
        public string IPAF_Display_Name { get; set; }

        public int IPAF_Active_Flag { get; set; }

        public Array AdditionalList { get; set; }

        public Array editingList { get; set; }
        public string message { get; set; }
        public bool returnval { get; set; }
        public long MI_Id { get; set; }

    }
}
