using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.PDA
{
    public class PDAExpenditureHeadDTO:CommonParamDTO
    {
        public long PDAEH_Id { get; set; }
        public long PDAE_Id { get; set; }
        public long PDAMH_Id { get; set; }
        public decimal PDAEH_Amount { get; set; }
        public string PDAE_Remarks { get; set; }

        public long AMST_Id { get; set; }
    }
}
