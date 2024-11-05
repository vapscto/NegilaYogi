using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class Fee_Groupwise_AutoReceiptDTO
    {
        public long FGAR_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string FGAR_PrefixFlag { get; set; }
        public string FGAR_PrefixName { get; set; }
        public string FGAR_SuffixFlag { get; set; }
        public string FGAR_SuffixName { get; set; }
        public string FGAR_Name { get; set; }
        public string FGAR_Address { get; set; }

        public long FGARG_Id { get; set; }
        public long FMG_Id { get; set; }

        public long userid { get; set; }

        public long roleid { get; set; }

        public Array acayear { get; set; }

        public Array fillgroup { get; set; }

        public Fee_Groupwise_AutoReceipt_GroupsDTO[] savegroup { get; set; }

        public string returnvalue { get; set; }
        public string returnvaluetwo { get; set; }
        public Array filldata { get; set; }

        public string FMG_GroupName { get; set; }
        public string academicyear { get; set; }
        public long FGAR_Starting_No { get; set; }
        public string FGAR_Template_Name { get; set; }

        public string htmldata { get; set; }
        public Array get_grpDetail { get; set; }
        


    }

    public class Fee_Groupwise_AutoReceipt_GroupsDTO
    {
        public long FMG_Id { get; set; }
        public string FMGG_GroupName { get; set; }
    }
}
