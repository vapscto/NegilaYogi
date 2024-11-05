using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Fees.FinancialAccounting
{
   public class FAUser_GroupDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public string returnval { get; set; }
        public Array getgroupname { get; set; }
        public Array getreport { get; set; }
        public long FAUGRP_Id { get; set; }
        
        public long FAMGRP_Id { get; set; }
        public long FAUGRP_ParentId { get; set; }
        public long FAUGRP_IdTwo { get; set; }
        public string FAUGRP_UserGroupName { get; set; }
        public string FAUGRP_AliasName { get; set; }
        public string FAUGRP_Description { get; set; }
        public long FAMCOMP_Id { get; set; }
        public long IMFY_Id { get; set; }
        public long FAUGRP_Position { get; set; }
        public bool FAUGRP_ActiveFlg { get; set; }
        public string FAMCOMP_CompanyName { get; set; }
        public string FAMGRP_GroupName { get; set; }
        public Array fyear { get; set; }
        public Array companyname { get; set; }
        public long FAUGRP_Idone { get; set; }
      
        //public long FAUGRP_Idtwo { get; set; }
        public Array usergroupname { get; set; }
        public Array getreporttwo { get; set; }
    }
}
