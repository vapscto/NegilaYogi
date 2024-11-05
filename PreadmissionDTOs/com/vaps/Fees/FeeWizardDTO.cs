using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeWizardDTO
    {
        public long FMG_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMG_GroupName { get; set; }

        public long FYG_Id { get; set; }

        public long ASMAY_Id { get; set; }
        public long ASMAY_Idnew { get; set; }
       
        public long user_id { get; set; }

        public bool returnval { get; set; }
     
        public Array groupYearData { get; set; }

        //  public Array tetmarray { get; set; }
        public Array retrivedata { get; set; }

        public bool retflg { get; set; }

        public string message { get; set; }
        public string ASMAY_Name { get; set; }

        public Array fillmastergroup { get; set; }
        public TempDTOO[] TempararyArrayList { get; set; }

        public Array getarray { get; set; }
        public Array academicdrp { get; set; }
        public Array academicyearnew { get; set; }
        public Array yearlygroup { get; set; }
        public Array yearlygrouphead { get; set; }
        public Array yearlygroupheaddata { get; set; }
        public long ASMAY_Order { get; set; }
        public FeeWizardDTO[] resultData { get; set; }
        public string returnduplicatestatus { get; set; }

        public bool FYG_ActiveFlag { get; set; }
        public long FMH_Id { get; set; }
        public long FMI_Id { get; set; }
        public string FYGHM_FineApplicableFlag { get; set; }
        public string FYGHM_Common_AmountFlag { get; set; }
        public string FYGHM_ActiveFlag { get; set; }
        public long FYGHM_Id { get; set; }
        public string FMH_FeeName { get; set; }
        public string FMI_Name { get; set; }


        public Array classcategory { get; set; }

        public Array classcategorydata { get; set; }
        public string FMCC_ClassCategoryName { get; set; }
        public string FMCC_ClassCategoryCode { get; set; }
        public string ASMCL_ClassName { get; set; }
        public bool FMCC_ActiveFlag { get; set; }
        public bool FYCC_ActiveFlag { get; set; }
        public long FYCCC_Id { get; set; }

        public long FYCC_Id { get; set; }
        public long FMCC_Id { get; set; }
        public long ASMCL_Id { get; set; }


        public string FTI_Name { get; set; }

        public long FMA_Id { get; set; }
        public long FTI_Id { get; set; }

        public decimal FMA_Amount { get; set; }
        public Array amountentrydata { get; set; }

        public Array amountentry { get; set; }
        public Array autoreceiptdata { get; set; }
        public Array autoreceipt { get; set; }

        public DateTime? FMA_DueDate { get; set; }

        public string FGAR_PrefixName { get; set; }
        public string FGAR_SuffixName { get; set; }
        public long FGAR_Starting_No { get; set; }
        public long FGAR_Id { get; set; }
        public long roleid { get; set; }

        public string FGAR_PrefixFlag { get; set; }

        public string FGAR_SuffixFlag { get; set; }
        public string FGAR_Template_Name { get; set; }
        public string FGAR_Name { get; set; }
        public string FGAR_Address { get; set; }
    }
}
