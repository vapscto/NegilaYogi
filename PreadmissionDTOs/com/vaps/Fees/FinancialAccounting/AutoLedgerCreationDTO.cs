using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Fees.FinancialAccounting
{
    public class AutoLedgerCreationDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public Array getreport { get; set; }
        public Array getgroupname { get; set; }
        public Array companyname { get; set; }
        public Array fyear { get; set; }
        public long FAMCOMP_Id { get; set; }
        public long FAMGRP_Id { get; set; }
        public long FAMLED_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long IMFY_Id { get; set; }

        public Array supplierdata { get; set; }
        public Array studentdata { get; set; }
        public Array itemdata { get; set; }
        public Array classarr { get; set; }
        public Array sectionarr { get; set; }
        public Array editledger { get; set; }
        public Array editledgerdetail { get; set; }
        public string returnval { get; set; }

        public long[] Amstid { get; set; }
        public long[] itemid { get; set; }
        public long[] salesid { get; set; }

        public string type { get; set; }
        public string crdrflg { get; set; }
      
    }
}
