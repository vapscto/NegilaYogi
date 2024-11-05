using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.PDA
{
    public class PDAExpenditureDTO:CommonParamDTO
    {
      public long PDAE_Id { get; set; }
      public long MI_Id { get; set; }

      public long  AMST_Id { get; set; }
      public DateTime PDAE_Date { get; set; }
      public string PDAE_TransactionNo { get; set; }

      public decimal PDAE_TotAmount { get; set; }
      public long ASMAY_Id { get; set; }

        public bool returnresult { get; set; }
        public bool returnval { get; set; }

    }
}
