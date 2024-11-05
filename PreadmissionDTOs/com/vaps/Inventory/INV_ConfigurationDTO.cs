using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class INV_ConfigurationDTO
    {
        public string message { get; set; }
        public long INVC_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMST_Id { get; set; }
        public string INVC_LIFOFIFOFlg { get; set; }
        public bool INVC_ProcessApplFlg { get; set; }
        public bool INVC_PRApplicableFlg { get; set; }
        public int INVC_AlertsBeforeDays { get; set; }
        public Array get_store { get; set; }
        public Array get_configdetails { get; set; }
        public selectedStoreDTO[] selectedStore { get; set; }
    }

    public class selectedStoreDTO
    {
        public long INVMST_Id { get; set; }
    }

}
