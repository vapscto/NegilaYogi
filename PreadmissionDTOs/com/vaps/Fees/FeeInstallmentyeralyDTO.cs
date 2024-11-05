using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeInstallmentyeralyDTO:CommonParamDTO
    {
        public long FTI_Id { get; set; }
        public long FMI_Id { get; set; }
        public string FTI_Name { get; set; }
        public instemp[] TempararyArrayList { get; set; }
        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }
        public Array yrlData { get; set; }

        public Array datasendhtml { get; set; }
        public long valueloop { get; set; }
        public string fname1 { get; set; }
        public long fno1 { get; set; }
        public DateTime fdate { get; set; }
        public DateTime tdate { get; set; }
        public DateTime Aplc { get; set; }
        public DateTime ddate { get; set; }
        public Array cotorlsbind { get; set; }

        public long ASMAY_Id { get; set; }

        public long MI_Id { get; set; }

        public Array fillsaveddata { get; set; }


        public string instlreturn { get; set; }



    }
}
