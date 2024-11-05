using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeAmountEntryDTO
    {
        public long FMA_Id { get; set; }
        public long MI_Id { get; set; }
        public long FMG_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long FMCC_Id { get; set; }
        public long FTI_Id { get; set; }
        public decimal FMA_Amount { get; set; }
        public string FMA_Flag { get; set; }

        public int FMH_Order { get; set; }

        public string FTDD_Month { get; set; }
        public string FTDD_Day { get; set; }

        public string FTDDE_Month { get; set; }
        public string FTDDE_Day { get; set; }

        public long FMH_Id { get; set; }
        public string FTI_Name { get; set; }

        public Array fillmastergroup { get; set; }
        public Array fillcompany { get; set; }
        public Array fillmasterhead { get; set; }
        public Array fillinstallment { get; set; }
        public Array fillcategory { get; set; }

        public Array fillmonth { get; set; }
        public Array fillmonthecs { get; set; }
        public string returnval { get; set; }
        public Array alldata { get; set; }

        public Array fillslab { get; set; }
        public Array fillslabecs { get; set; }

        public Array allgroupheaddata { get; set; }
        public string FMG_GroupName { get; set; }
        public string FMH_FeeName { get; set; }
        public string FMI_Name { get; set; }
        public FeeAmountEntryDTO[] savetmpdata { get; set; }
        public FeeTFineSlabDTO[] savefineslabreg { get; set; }
        public FeeTFineSlabECSDTO[] savefineslabecs { get; set; }

        public string regularfalg { get; set; }
        public string ecsflag { get; set; }

        public Array fineslabdetails { get; set; }
        public Array fineslabdetailsecs { get; set; }
        //added on 19 dec 2016
        public decimal FTFS_Amount { get; set; }
        public string FMFS_FineType { get; set; }
        public string FMFS_ECSFlag { get; set; }
        public string FTFS_FineType { get; set; }

        public long FMFS_FromDay { get; set; }
        public long FMFS_ToDay { get; set; }

        public decimal FTFSE_Amount { get; set; }
        public string FTFSE_FineType { get; set; }

        public long FYGHM_Id { get; set; }

        public Array commountamountflag { get; set; }
        //mb
        public int AMM_Month_Max_Days { get; set; }
        public int AMME_Month_Max_Days { get; set; }
        public string ASMAY_Year { get; set; }
        public string FMCC_ClassCategoryName { get; set; }
        public string amtentrystatus { get; set; }
        public string FYGHM_FineApplicableFlag { get; set; }
        public Array academicdrp { get; set; }

        public string selectiontype { get; set; }

        //Other Staff Fee Amount Entry .
        public long FMAOST_Id { get; set; }
       public decimal FMAOST_Amount { get; set; }
       public int FTDD_Year { get; set; }
        public long FTFSOST_Id { get; set; }
        public long FMFS_Id { get; set; }
        public string FTFSOST_FineType { get; set; }
        public decimal FTFSOST_Amount { get; set; }

        public Array newlyupdatedrec { get; set; }

        public long FTFSE_Id { get; set; }

        public long user_id { get; set; }

        public Array feeconfiguration { get; set; }

        public DateTime Duedate { get; set; }

        public DateTime? FMA_DueDate { get; set; }
        public DateTime? FTDDE_DueDate { get; set; }
        public DateTime? FMA_PartialRebateApplicableDate { get; set; }
    }
}
