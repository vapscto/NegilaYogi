using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class INV_Master_StoreDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
     
        public long INVMST_Id { get; set; }
        public long MI_Id { get; set; }
        public string INVMS_StoreName { get; set; }
        public string INVMS_StoreLocation { get; set; }
        public bool INVMS_ActiveFlg { get; set; }
        public string INVMS_ContactPerson { get; set; }
        public long INVMS_ContactNo { get; set; }
        public Array get_store { get; set; }
        public Array empname_list { get; set; }
        public long HRME_Id { get; set; }
        public string employeename { get; set; }


    }
}
