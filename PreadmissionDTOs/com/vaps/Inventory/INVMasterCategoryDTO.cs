using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class INVMasterCategoryDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        public Array categorylist { get; set; }

        public long INVMC_Id { get; set; }
        public long MI_Id { get; set; }
        public long User_Id { get; set; }
        public string INVMC_CategoryName { get; set; }
        public string INVMC_AliasName { get; set; }
        public string INVMC_ParentId { get; set; }
        public long INVMC_Level { get; set; }
        public bool INVMC_ActiveFlg { get; set; }
        public temp_mastercat[] ordeidss { get; set; }


    }

    public class temp_mastercat
    {
        public long INVMC_Id { get; set; }
        public long INVMC_Level { get; set; }
    }
}
