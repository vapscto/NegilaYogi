using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class INV_Master_TaxDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }

        public long INVMT_Id { get; set; }
        public long MI_Id { get; set; }
        public string INVMT_TaxName { get; set; }
        public string INVMT_TaxAliasName { get; set; }
        public bool INVMT_ActiveFlg { get; set; }
        public Array get_tax { get; set; }


    }
}
