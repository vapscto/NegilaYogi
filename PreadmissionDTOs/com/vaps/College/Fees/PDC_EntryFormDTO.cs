using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Fees
{
    public class PDC_EntryFormDTO
    {
        public long FCSPDC_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public string FCSPDC_ChequeNo { get; set; }
        public DateTime? FCSPDC_ChequeDate { get; set; }
        public decimal FCSPDC_Amount { get; set; }
        public long FMBANK_Id { get; set; }
        public string FCSPDC_Currency { get; set; }
        public string FCSPDC_Narration { get; set; }
        public string FCSPDC_Status { get; set; }
        public bool FCSPDC_ActiveFlg { get; set; }
        public DateTime FCSPDC_CreatedDate { get; set; }
        public DateTime FCSPDC_UpdatedDate { get; set; }
        public long FCSPDC_CreatedBy { get; set; }
        public long FCSPDC_Updatedby { get; set; }

        public long user_id { get; set; }

        public string returnduplicatestatus { get; set; }

        public string message { get; set; }

        public Array Fillcourse { get; set; }

        public Array fillbranch { get; set; }

        public Array FillSemester { get; set; }

        public Array FillCategory { get; set; }
        public Array FillBank { get; set; }

        public string confirmmgs { get; set; }

        public string returnval { get; set; }
        public long ASMAY_Id { get; set; }

        public Array pdcrecord { get; set; }

        public Array GroupData { get; set; }

        public string AMCST_FirstName { get; set; }

        public Array admsudentslist { get; set; }

        public PDC_EntryFormDTO[] savetmpdata { get; set; }

        public string AMSE_SEMName { get; set; }

        public Array remainders { get; set; }

        public long AMCST_MobileNo  { get;set;}

        public string AMCST_emailId { get; set; }

        public Array yearlst { get; set; }
        public Array grouplist { get; set; }
        public Array fillfeehead { get; set; }
        public Array courselist { get; set; }

        public Array branchlist { get; set; }
        public Array semisterlist { get; set; }

        public Array semisterlistnew { get; set; }

        public Array alldata { get; set; }
        public FeeGroupDTO[] TempararyArrayList { get; set; }

        public long FMH_Id { get; set; }
        public string FMH_FeeName { get; set; }
        public FeeHeadDTO[] TempararyArrayheadList { get; set; }

        public Array alldatahead { get; set; }
        public long[] AMCO_Ids { get; set; }
        public long[] AMB_Ids { get; set; }
        public long[] AMSE_Ids { get; set; }

        public string flag { get; set; }
        public DateTime fromdate { get; set; }

        public DateTime todate { get; set; }

        public Array savedrecord { get; set; }



    }
}
