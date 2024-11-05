using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class INV_Master_SupplierDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }

        public long INVMS_Id { get; set; }
        public long MI_Id { get; set; }
        public string INVMS_SupplierName { get; set; }
        public string INVMS_SupplierCode { get; set; }
        public string INVMS_SupplierConatctPerson { get; set; }
        public long INVMS_SupplierConatctNo { get; set; }
        public string INVMS_EmailId { get; set; }
        public string INVMS_GSTNo { get; set; }
        public string INVMS_SupplierAddress { get; set; }
        public bool INVMS_ActiveFlg { get; set; }

        public Array get_supplier { get; set; }


    }
}
