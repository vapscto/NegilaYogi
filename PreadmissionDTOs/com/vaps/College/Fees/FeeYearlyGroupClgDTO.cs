using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Fee
{
    public class FeeYearlyGroupClgDTO:CommonParamDTO
    {
        public long FYG_Id { get; set; }
        public long FMG_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public bool FYG_ActiveFlag { get; set; }
        public long user_id { get; set; }

        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }

        public Array groupYearData { get; set; }

      //  public Array tetmarray { get; set; }
        public Array retrivedata { get; set; }



        public TempDTOO[] TempararyArrayList { get; set; }

        public string grpname { set; get; }
        public string yearname { set; get; }

        public bool retflg { get; set; }

        public string message { get; set; }

        public Array fillmastergroup { get; set; }

    }
}
