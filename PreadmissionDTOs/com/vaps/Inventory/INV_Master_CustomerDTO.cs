using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class INV_Master_CustomerDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }

        public long INVMC_Id { get; set; }
        public long MI_Id { get; set; }
        public string INVMC_CustomerName { get; set; }
        public string INVMC_CustomerContactPerson { get; set; }
        public long INVMC_CustomerContactNo { get; set; }
        public string INVMC_CustomerAddress { get; set; }
        public bool INVMC_ActiveFlg { get; set; }
        public Array get_customer { get; set; }


    }
}
