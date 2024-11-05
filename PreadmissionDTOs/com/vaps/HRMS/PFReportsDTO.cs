using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class PFReportsDTO
    {
        public long MI_Id { get; set; }
        public string retrunMsg { get; set; }
        public long roleId { get; set; }

        public string HRME_EmployeeCode { get; set; }
        public bool? HRME_EPFNotApplicableFlg { get; set; }
        public bool? HRME_RetiredFlg { get; set; }
        public bool? HRME_FPFNotApplicableFlg { get; set; }
        public DateTime? HRME_PensionStoppedDate { get; set; }
        public double? HRES_WorkingDays { get; set; }

        public string departmentname { get; set; }

        public Array employeedropdown { get; set; }
        public Array leaveyeardropdown { get; set; }
        public Array monthdropdown { get; set; }
        public long HRME_Age { get; set; }

        public long HRME_Id { get; set; }
        public string HRES_Year { get; set; }
        public string HRES_Month { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public Array getpfgriddata { get; set; }
        public Array pfreport { get; set; }
        public Array employeeDetails { get; set; }
        public Array contactnum { get; set; }

        public Array institutionDetails { get; set; }

        public string MI_Name { get; set; }
        public string MI_Address1 { get; set; }
        public string finYearFromDate { get; set; }
        public string finYearToDate { get; set; }
        public string HeadType { get; set; }
        public string PFVPFflag { get; set; }
        public string DepositWithdrow { get; set; }
        public string Remark { get; set; }
        public long IMFY_Id { get; set; }
        public long IVRM_Month_Id { get; set; }
        public long UserId { get; set; }
        public decimal? TransAmount { get; set; }
        public decimal? Schoolamount { get; set; }
        public string Procedure { get; set; }
        public string Flag { get; set; }
        public Array EmployeePFreportDetails { get; set; }
          public long TransactionID { get; set; }
        //pra
        public decimal? STJOwnPF { get; set; }
        public long HRESD_Id { get; set; }
        public long HRMED_Id { get; set; }
        public string HRMED_Name { get; set; }
        public decimal? HRESD_Amount { get; set; }
        public DateTime? HRME_DOJ { get; set; }
        public decimal? AmountofWages { get; set; }
        public decimal? PFAmount { get; set; }

        public long HRES_Id { get; set; }
        public decimal? HRES_EPF { get; set; }
        public decimal? HRES_FPF { get; set; }
        public decimal? VPFAmount { get; set; }
        public Array PayrollStandard { get; set; }
        public int? HRC_RetirementYrs { get; set; }
        public string FatherHusbandName { get; set; }

        public string HRME_PFAccNo { get; set; }
        public string HRME_FatherName { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }

        public DateTime? HRME_DOL { get; set; }
        public string HRME_LeavingReason { get; set; }

        public DateTime? HRME_PFDate { get; set; }
        public DateTime? HRME_DOB { get; set; }
        public string IVRMMG_GenderName { get; set; }
        public string TotalServicePeriod { get; set; }


        public decimal? HRES_Ac21 { get; set; }
        public decimal? HRES_Ac22 { get; set; }
        public decimal? HRES_Ac5 { get; set; }
        public decimal? HRES_Ac10 { get; set; }

        public string HRMED_EDTypeFlag { get; set; }

        public Array bankdetails { get; set; }

        public decimal? professionaltaxamount { get; set; }

        public string HRME_PFuAN { get; set; }

        public decimal? empGrossSal { get; set; }

        public decimal? netsalary { get; set; }
        public long?[] groupTypeIdList { get; set; }
        public long?[] hrmgT_IdList { get; set; }
        public long?[] employeeTypeIdList { get; set; }
        public long?[] hrmD_IdList { get; set; }
        public long?[] hrmdeS_IdList { get; set; }
        
        public int abc { get; set; }
        public MasterEmployeeDTO currentemployeeDetails { get; set; }


        public PFReportsDTO[] employee { get; set; }

        public int? H_DOB { get; set; }
        public long H_Month { get; set; }
        public long basicamount { get; set; }
        public long DAamount { get; set; }
        public long Othersamount { get; set; }
        public decimal? emptotdedSal { get; set; }

        public decimal? basicvalue { get; set; }
        public decimal? davalue { get; set; }
        public long HRMS_Age { get; set; }

        public long HRMPFVPFINT_Id { get; set; }

        public decimal HRMPFVPFINT_PFInterestRate { get; set; }
        public decimal HRMPFVPFINT_VPFInterestRate { get; set; }
        public bool HRMPFVPFINT_ActiveFlg { get; set; }
        public DateTime HRMPFVPFINT_CreatedDate { get; set; }
        public DateTime HRMPFVPFINT_UpdatedDate { get; set; }
        public long HRMPFVPFINT_CreatedBy { get; set; }
        public long HRMPFVPFINT_UpdatedBy { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public string message { get; set; }
        public Array get_store { get; set; }
        public string IMFY_FinancialYear { get; set; }


        public vpfTranaction[] vpfTranaction { get; set; }
        public pfTranaction[] pfTranaction { get; set; }

    }

    public class vpfTranaction
    {
        public long HREVPFST_Id { get; set; }
        public decimal HREVPFST_VOBAmount { get; set; }
        public decimal HREVPFST_Contribution { get; set; }
        public decimal HREVPFST_Intersest { get; set; }
        public decimal HREVPFST_TransferAmount { get; set; }
        public decimal HREVPFST_WithdrawnAmount { get; set; }
        public decimal HREVPFST_SettledAmount { get; set; }
        public decimal HREVPFST_DepositAdjustmentAmount { get; set; }
        public decimal HREVPFST_WithsrawAdjustmentAmount { get; set; }
        public decimal HREVPFST_ClosingBalance { get; set; }
    }

    public class pfTranaction
    {
        public long HREPFST_Id { get; set; }
        public decimal HREPFST_OBInstituteAmount { get; set; }
        public decimal HREPFST_OBOwnAmount { get; set; }
        public decimal HREPFST_OwnContribution { get; set; }
        public decimal HREPFST_IntstituteContribution { get; set; }
        public decimal HREPFST_OwnInterest { get; set; }
        public decimal HREPFST_InstituteInterest { get; set; }
        public decimal HREPFST_OwnTransferAmount { get; set; }
        public decimal HREPFST_InstituteTransferAmount { get; set; }
        public decimal HREPFST_OwnWithdrwanAmount { get; set; }
        public decimal HREPFST_InstituteWithdrawnAmount { get; set; }
        public decimal HREPFST_OwnSettlementAmount { get; set; }
        public decimal HREPFST_InstituteLSettlementAmount { get; set; }
        public decimal HREPFST_OwnDepositAdjustmentAmount { get; set; }
        public decimal HREPFST_InstituteDepositAdjustmentAmount { get; set; }
        public decimal HREPFST_OwnWithdrawAdjustmentAmount { get; set; }
        public decimal HREPFST_InstituteWithdrawAdjustmentAmount { get; set; }
        public decimal HREPFST_OwnClosingBalance { get; set; }
        public decimal HREPFST_InstituteClosingBalance { get; set; }
    }
}
