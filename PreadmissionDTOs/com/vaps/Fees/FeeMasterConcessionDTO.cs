using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeMasterConcessionDTO
    {
        public long FMCC_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMCC_ConcessionName { get; set; }
        public string FMCC_ConcessionFlag { get; set; }
        public string FMCC_ConcessionApplLimit { get; set; }
        public bool FMCC_ActiveFlag { get; set; }
         public Array savedata { get; set; }
        public Array editdata { get; set; }
        public Array feeconcess { get; set; }
        public string returnduplicatestatus { get; set; }
        public Array ClaSSCategoryArray { get; set; }
        public bool retrunval { get; set; }
        public string message { get; set; }




      public Array concession { get; set; }
      public long FMCCD_Id { get; set; }
      public long FMCCD_FromNoSibblings { get; set; }
      public long FMCCD_ToNoSibblings { get; set; }
      public decimal FMCCD_PerOrAmt { get; set; }
        public string FMCCD_PerOrAmtFlag { get; set; }
        public Array savedata22 { get; set; }
        public bool FMCCD_ActiveFlg { get; set; }
        public Array editdata2 { get; set; }
        public long FMG_Id { get; set; }
        public string FMG_GroupName { get; set; }
        public Array group { get; set; }
        public long FMH_Id { get; set; }
        public string FMH_FeeName { get; set; }
        public Array head { get; set; }
        public long FMACCG_Id { get; set; }
        public Array savedata33 { get; set; }
        public Array concession3 { get; set; }
        public Array editdata3 { get; set; }
        public Array editlist3 { get; set; }
        public bool FMACCG_ActiveFlg { get; set; }
        public FeeMasterConcessionDTO[] headlistdata { get; set; }
    }
}
