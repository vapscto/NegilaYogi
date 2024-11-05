using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeesMakerAndCheckerDTO
    {
        public long FYPAPP_Id { get; set; }
        public long FYP_Id { get; set; }
        public long HRME_Id { get; set; }
        public bool FYPAPP_ApprovedFlg { get; set; }
        public string FYPAPP_Remarks { get; set; }
        public DateTime FYPAPP_DateTime { get; set; }
        public bool FYPAPP_ActiveFlg { get; set; }
        public DateTime FYPAPP_CreatedDate { get; set; }
        public DateTime FYPAPP_UpdatedDate { get; set; }
        public long FYPAPP_CreatedBy { get; set; }
        public long FYPAPP_UpdatedBy { get; set; }

        public long MI_ID { get; set; }

        public string returnval { get; set; }
        public Array feeconfiguration { get; set; }
        public long userid { get; set; }
        public Array feeconfiglist { get; set; }
        public long roleid { get; set; }
        public Array adcyear { get; set; }

        public DateTime fromdate { get; set; }

        public DateTime todate { get; set; }

        public string Remark { get; set; }
        public Array feepaymentreport { get; set; }


        public Array fillstudentviewdetails { get; set; }


        public string HRME_EmployeeFirstName { get; set; }
        public long AMCST_Id { get; set; }
        public long FMG_Id { get; set; }
        public long FCMAS_Id { get; set; }
        public long FMH_Id { get; set; }
        public long FTI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long FCSS_ToBePaid { get; set; }
        public long FCSS_PaidAmount { get; set; }
        public long FCSS_ConcessionAmount { get; set; }
        public long FCSS_NetAmount { get; set; }
        public long FCSS_FineAmount { get; set; }
        public long FCSS_RefundAmount { get; set; }
        public string FMH_FeeName { get; set; }
        public string FTI_Name { get; set; }
        public string FMG_GroupName { get; set; }
        public long FCSS_CurrentYrCharges { get; set; }
        public long FCSS_TotalCharges { get; set; }
        public long FCSS_OBArrearAmount { get; set; }
        public long FCSS_WaivedAmount { get; set; }
        public long FMH_Order { get; set; }
        public FeesMakerAndCheckerDTO[] studentdata { get; set; }

        public Array courselist { get; set; }

        public Array grouplist { get; set; }

        public Array branchlist { get; set; }

        public Array semisterlist { get; set; }

        public Array semisterlistnew { get; set; }
        public long[] AMCO_Ids { get; set; }
        public long[] AMB_Ids { get; set; }
        public long[] AMSE_Ids { get; set; }
        public FeeGroupDTO[] TempararyArrayList { get; set; }

        public Array alldata { get; set; }
        public FeeHeadDTO[] TempararyArrayheadList { get; set; }

        public Array alldatahead { get; set; }
        public long[] FMG_Ids { get; set; }

        public string allorindivflag { get; set; }

        public string allorstdorothersflag { get; set; }

        public string allorcorchoronlineflag { get; set; }

        public long cheque { get; set; }


        public Array savedrecord { get; set; }
        public Array fillfeehead { get; set; }


        public Array yearlst { get; set; }

        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }

        public Array fillstudentviewdetailsadvance { get; set; }

        public Array paymentremark { get; set; }

        public string FTP_Paid_Amt { get; set; }

        public String username { get; set; }

        public Array Approvedbyname { get; set; }

        public string EmpName { get; set; }

        public string modeofpayment { get; set; }

        public string fyppM_BankName { get; set; }

        public string fyppM_DDChequeNo { get; set; }
        public DateTime fyppM_DDChequeDate { get; set; }

        public DateTime fyppM_ClearanceDate { get; set; }

        public string overalltot { get; set; }
    }
}
