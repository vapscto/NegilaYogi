using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Fees
{
     public  class ClgQuotaFeeGroupDTO
    {

     public long FCQCFG_Id { get; set; }

        public long MI_Id { get; set; }
        public long ACQC_Id { get; set; }

        public long FMG_Id { get; set; }

        public bool FCQCFG_CompulsoryFlg { get; set; }
        
        public bool FCQCFG_ActiveFlg { get; set; }

        public DateTime? FCQCFG_CreatedDate { get; set; }
        
        public DateTime? FCQCFG_UpdatedDate { get; set; }

        public long FCQCFG_CreatedBy{ get; set; }
        
        public long FCQCFG_UpdatedBy{ get; set; }

        public long ASMAY_Id { get; set; }

        public long user_id { get; set; }
        public Array feegroup { get; set; }
        public Array Category { get; set; }

        public Array GroupData { get; set; }

        public String returnduplicatestatus { get; set; }

        public String message { get; set; }

        public String ACQC_CategoryName { get; set; }

        public string FMG_GroupName { get; set; }



        public string returnval { get; set; }

        public string confirmmgs { get; set; }

        public fmg_id[] TempararyArrayList { get; set; }




    }

    public class fmg_id
    {

        public long fmG_Id { get; set; }
    }
}
