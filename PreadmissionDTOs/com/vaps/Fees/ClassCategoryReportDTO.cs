using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class ClassCategoryReportDTO
    {
        public long ASMAY_Id { get; set; }

        public long MI_Id { get; set; }

        public Array YearList { get; set; }
        public string FMCC_ClassCategoryName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string ASMCL_ClassName { get; set; }
        public Array searchdatalist { get; set; }

        public string showreport { get; set; }

        public long type { get; set; }


        public long FMG_Id { get; set; }
        public string FMG_GroupName { get; set; }


        public bool FMH_RefundFlag { get; set; }
        public bool FMH_PDAFlag { get; set; }
        public bool FMH_SpecialFeeFlag { get; set; }
        public long FMH_Order { get; set; }
        public string FMH_FeeName { get; set; }
        public string FMH_Flag { get; set; }
        public long FMH_Id { get; set; }
        public long FMI_Id { get; set; }
        public string FMI_Name { get; set; }
        public string FYGHM_FineApplicableFlag { get; set; }
        public string FYGHM_Common_AmountFlag { get; set; }

        public long FTI_Id { get; set; }

        public string FTI_Name { get; set; }

        public DateTime FTIDD_ApplicableDate { get; set; }

        public DateTime FTIDD_DueDate { get; set; }

        public DateTime FTIDD_FromDate { get; set; }

        public DateTime FTIDD_ToDate { get; set; }
        public long FMFS_Id { get; set; }
        public string FMFS_FineType { get; set; }
        public long FMFS_FromDay { get; set; }
        public long FMFS_ToDay { get; set; }
        public string FMFS_ECSFlag { get; set; }
        public bool FMFS_ActiveFlag { get; set; }

        public decimal FMA_Amount { get; set; }

        public long FMA_Id { get; set; }

        public decimal FTFSE_Amount { get; set; }

        public long FTDDE_Month { get; set; }
        public string FMGG_GroupName { get; set; }
        public long FMGG_Id { get; set; }

        public long ASMS_Id { get; set; }
        public long ASMCL_Id { get; set; }

        public long FMCC_Id { get; set; }


        public string NormalizedUserName { get; set; }

        public string IVRMRT_Role { get; set; }
        public string User_Id { get; set; }


        public long FMT_Id { get; set; }
        public long FMT_Name { get; set; }





    }
}
