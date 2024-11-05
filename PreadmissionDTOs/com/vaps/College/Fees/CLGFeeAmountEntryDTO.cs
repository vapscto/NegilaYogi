using PreadmissionDTOs.com.vaps.College.Fee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Fees
{
    public class CLGFeeAmountEntryDTO
    {

        public long FCMA_Id { get; set; }

        public long MI_Id { get; set; }

        public long ASMAY_Id { get; set; }

        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long FMG_Id { get; set; }
        public long FMH_Id { get; set; }
        public long FTI_Id { get; set; }
        public string FCMA_Flag { get; set; }
        public bool FCMA_ActiveFlg { get; set; }

        public Array academicdrp { get; set; }

        public Array currfillyear { get; set; }

        public Array Fillcourse { get; set; }

        public string AMB_BranchName { get; set; }


        public Array Branch { get; set; }

        public Array Fillgroup { get; set; }

        public Array fillbranch { get; set; }
        public Array fillsemester { get; set; }
        public Array fillseatdistr { get; set; }

        public long AMSE_Id { get; set; }

        public Array commountamountflag { get; set; }

        public Array newlyupdatedrec { get; set; }

        public string FMG_GroupName { get; set; }
        public decimal FCMAS_Amount { get; set; }

        public string FMI_Name { get; set; }
        public string FMH_FeeName { get; set; }
        public int FMH_Order { get; set; }
        public string FYGHM_FineApplicableFlag { get; set; }

        public Array allgroupheaddata { get; set; }

        public long FCMAS_Id { get; set; }

        public string FCTDD_Day { get; set; }
        public string FCTDD_Month { get; set; }
        public string FTDD_Month { get; set; }

        public Array fineslabdetails { get; set; }

        public decimal FCTFS_Amount { get; set; }
        public string FMFS_FineType { get; set; }

        public string FMFS_ECSFlag { get; set; }

        public string FCTFS_FineType { get; set; }

        public long FMFS_FromDay { get; set; }
        public long FMFS_ToDay { get; set; }

        public Array fillmonth { get; set; }

        public Array fillslab { get; set; }

        public Array getloanscholership { get; set; }

        public CLGFeeAmountEntryDTO[] savetmpdata { get; set; }
        public Fee_College_T_Fine_SlabsDTO[] savefineslabreg { get; set; }

        public string returnval { get; set; }
        public string amtentrystatus { get; set; }

        public Array alldata { get; set; }

        public string FTI_Name { get; set; }

        public decimal FTFS_Amount { get; set; }

        public string FTFS_FineType { get; set; }
        
        public long FMA_Id { get; set; }
        public decimal FMA_Amount { get; set; }
        public Array fillstudent { get; set; }
        public Array temp { get; set; }
        public long[] AMCO_Ids { get; set; }
        public long[] AMB_Ids { get; set; }
        public long[] AMSE_Ids { get; set; }
        public string Currencyfactor { get; set; }
        public long[] AMCST_Ids { get; set; }

        public bool returnresult { get; set; }
        public Array savedrecord { get; set; }
        public long AMCST_Id { get; set; }
        public long FCSA_Id { get; set; }
        public long FCSS_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public Array Fillcourseyearwise { get; set; }

        public long user_id { get; set; }

        public string FCTFS_PercentageFlg { get; set; }

        public Array fillallyear { get; set; }

        public string FCTDD_Year { get; set; }
        public Array searchdatalist { get; set; }

        public string AMSE_SEMName { get; set; }

        public long AMSE_SEMOrder { get; set; }
        public DateTime? FCMAS_DueDate { get; set; }
        public Array semlist { get; set; }
        public string selectiontype { get; set; }


    }
}
