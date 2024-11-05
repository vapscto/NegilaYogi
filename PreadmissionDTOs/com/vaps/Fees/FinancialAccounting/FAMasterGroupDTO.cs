using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Fees.FinancialAccounting
{
   public class FAMasterGroupDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public Array getreport { get; set; }
        public long FAMGRP_Id { get; set; }
        public long FAMGRP_IdTwo { get; set; }
        public string FAMGRP_GroupName { get; set; }

        public string FAMGRP_GroupCode { get; set; }
        public string FAMGRP_Description { get; set; }
        public string FAMGRP_BSPLFlg { get; set; }
        public string FAMGRP_CRDRFlg { get; set; }
        public string FAMGRP_Position { get; set; }
        public long FAMGRP_ParentId { get; set; }
        public bool FAMGRP_ActiveFlg { get; set; }
        public string returnval { get; set; }
        public Array getgroupname { get; set; }
        public Array getreporttwo { get; set; }
    }
}
