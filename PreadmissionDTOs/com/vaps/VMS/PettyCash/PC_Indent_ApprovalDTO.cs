using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager.PettyCash
{
    public class PC_Indent_ApprovalDTO
    {
        public long MI_Id { get; set; }
        public long Userid { get; set; }
        public long roleid { get; set; }
        public long ASMAY_Id { get; set; }
        public long HRME_Id { get; set; }
        public long PCINDENTAP_Id { get; set; }
        public string PCINDENTAPT_IndentNo { get; set; }
        public DateTime? PCINDENTAPT_Date { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? PCINDENT_Date_From { get; set; }
        public DateTime? PCINDENT_Date_To { get; set; }
        public string PCINDENTAPT_Desc { get; set; }
        public string PCINDENT_Desc { get; set; }
        public decimal? PCINDENTAPT_RequestedAmount { get; set; }
        public decimal? PCINDENTAPT_SanctionedAmt { get; set; }
        public Array indentdetails { get; set; }
        public Array indentparticulardetails { get; set; }
        public long PCINDENTAPDT_Id { get; set; }
        public long PCREQTN_Id { get; set; }
        public long PCMPART_Id { get; set; }
        public decimal? PCINDENTAPDT_Amount { get; set; }
        public string PCINDENTAPDT_Remarks { get; set; }
        public string PCMPART_ParticularName { get; set; }
        public decimal? PCINDENTAPDT_ApprovedAmt { get; set; }
        public bool PCINDENTAPDT_ActiveFlg { get; set; }
        public PC_Indent_Approved_Details_DTO[] PC_Indent_Approved_Details_DTO { get; set; }
        public string PCINDENT_IndentNo { get; set; }
        public string departmentname { get; set; }
        public string employeename { get; set; }
        public DateTime? PCINDENT_Date { get; set; }
        public decimal? PCINDENT_RequestedAmount { get; set; }
        public decimal? PCINDENT_ApprovedAmt { get; set; }
        public long PCINDENT_Id { get; set; }
        public Array getloaddata { get; set; }
        public Array getuserinstitution { get; set; }
        public Array getinstitutiondetails { get; set; }
        public temp_indentid[] temp_indentid { get; set; }
        public long pcindentdeT_Id { get; set; }
        public decimal? PCINDENTDET_Amount { get; set; }
        public decimal? PCINDENTDET_ApprovedAmt { get; set; }
        public string PCINDENTDET_Remarks { get; set; }
        public bool PCINDENTDET_ActiveFlg { get; set; }
        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }
        public string Auto_id { get; set; }
        public string message { get; set; }
        public bool returnval { get; set; }
        public Array getviewdata { get; set; }
        public Array getindentapprovaldetails { get; set; }      
        public Array getparticularsdetails { get; set; }
        public Array getparticularsindentdetails { get; set; }
        public Array getindentdetails { get; set; }
        public decimal? PCINDENTAPDT_RequestedAmount { get; set; }
        public decimal? PCINDENTAPDT_SanctionedAmt { get; set; }
        public decimal? PCINDENTAPT_AmountSpent { get; set; }
        public decimal? PCINDENTAPT_BalanceAmount { get; set; }
        public decimal? PCINDENTAPDT_AmountSpent { get; set; }
        public decimal? PCINDENTAPDT_BalanceAmount { get; set; }

        // Expenditure
        public long PCEXPTR_Id { get; set; }        
        public decimal? PCEXPTR_Amount { get; set; }
        public string PCEXPTR_ExpenditureNo { get; set; }
        public DateTime? PCEXPTR_Date { get; set; }
        public string PCEXPTR_Desc { get; set; }
        public string PCEXPTR_ModeOfPayment { get; set; }
        public string PCEXPTR_ReferenceNo { get; set; }
        public bool PCEXPTR_ActiveFlg { get; set; }
        public string saveorupdate { get; set; }
        public string PCEXPTR_DeletedRemarks { get; set; }
        public Array getexpendituredetails { get; set; }
        public Array getexpenditureloaddata { get; set; }
    }
    public class PC_Indent_Approved_Details_DTO
    {
        public long PCINDENTAPDT_Id { get; set; }
        public long PCINDENTAP_Id { get; set; }
        public long PCMPART_Id { get; set; }
        public decimal? PCINDENTAPDT_RequestedAmount { get; set; }
        public string PCINDENTAPDT_Remarks { get; set; }
        public decimal? PCINDENTAPDT_SanctionedAmt { get; set; }
        public bool PCINDENTAPDT_ActiveFlg { get; set; }
    }
    public class temp_indentid
    {
        public long PCINDENT_Id { get; set; }
    }
}