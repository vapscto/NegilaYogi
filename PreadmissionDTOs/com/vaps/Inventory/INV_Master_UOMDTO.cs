using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class INV_Master_UOMDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }

        public long INVMUOM_Id { get; set; }
        public long MI_Id { get; set; }
        public string INVMUOM_UOMName { get; set; }
        public string INVMUOM_UOMAliasName { get; set; }
        public bool INVMUOM_ActiveFlg { get; set; }
        public string INVMUOM_Qty { get; set; }
        public Array get_uom { get; set; }
     

    }
}
