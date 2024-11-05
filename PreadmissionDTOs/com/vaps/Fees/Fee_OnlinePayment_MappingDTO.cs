using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class Fee_OnlinePayment_MappingDTO
    {
        public long FOPM_Id { get; set; }
        public long MI_Id { get; set; }
        public long FMH_Id { get; set; }
        public string FOPM_BankAccountName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long fpgd_id { get; set; }
        public long fmg_id { get; set; }
        public long fti_id { get; set; }
        public long fmt_id { get; set; }
        public Array institutionList { get; set; }
        public Array classList { get; set; }
        public Array subMerchantIdList { get; set; }
        public Array groupNameList { get; set; }
        public Array headNameList { get; set; }
        public Array installmentList { get; set; }
        public Array termList { get; set; }
        public Array feeonlinepaymentmappingList { get; set; }
        public int count { get; set; }
        public string returnval { get; set; }
        public string institutionName { get; set; }
        public string groupName { get; set; }
        public string headName { get; set; }
        public string installmentName { get; set; }
        public string termName { get; set; }
        public string className { get; set; }
        public string submerchandId { get; set; }

        public long ASMAY_Id { get; set; }
        public int headlistcount { get; set; }
        public int installmentlistcount { get; set; }
        public Array editdata { get; set; }
        public Fee_OnlinePayment_MappingDTO[] selectedheadList { get; set; }
        public string Isduplicate { get; set; }

        public Array fillyear { get; set; }

        public string ASMCL_ClassName { get; set; }

        public int ASMCL_Order { get; set; }

        public long ASMCL_ID { get; set; }

        public long user_id { get; set; }

    }
}
