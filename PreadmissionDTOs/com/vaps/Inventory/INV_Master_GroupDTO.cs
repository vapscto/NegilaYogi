using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class INV_Master_GroupDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }

        public long INVMG_Id { get; set; }
        public long MI_Id { get; set; }
        public string INVMG_GroupName { get; set; }
        public string INVMG_AliasName { get; set; }
        public string INVMG_GroupName_mn { get; set; }
        public long INVMG_ParentId { get; set; }
        public string INVMG_Level { get; set; }
        public string INVMG_MGUGIGFlg { get; set; }
        public bool INVMG_ActiveFlg { get; set; }
        public Array get_maingroup { get; set; }
        public Array get_maingroupdd { get; set; }
        
        public Array get_usergp { get; set; }

        public Array get_usergroup { get; set; }
        public Array get_itemgroup { get; set; }
        public Array maingrp { get; set; }
        public Array usergrp { get; set; }

        public string INVMG_GroupStartingNo { get; set; }
        public string INVMG_GroupSuffix { get; set; }
        public string INVMG_GroupPrefix { get; set; }
        public Array getusergroup { get; set; }


    }
}
