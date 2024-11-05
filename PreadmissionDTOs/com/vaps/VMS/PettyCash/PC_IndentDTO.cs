using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager.PettyCash
{
    public class PC_IndentDTO
    {
        public long PCINDENT_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long Userid { get; set; }
        public long ASMAY_Id { get; set; }
        public long roleid { get; set; }
        public long PCREQTN_Id { get; set; }
        public long pcreqtndeT_Id { get; set; }
        public long PCMPART_Id { get; set; }
        public long PCINDENTDET_Id { get; set; }
        public string PCINDENT_IndentNo { get; set; }
        public string saveorupdate { get; set; }
        public string Role_flag { get; set; }
        public DateTime? PCINDENT_Date { get; set; }
        public DateTime? PCREQTN_Date { get; set; }
        public DateTime? PCINDENT_Date_From { get; set; }
        public DateTime? PCINDENT_Date_To { get; set; }
        public string PCINDENT_Desc { get; set; }
        public string PCMPART_ParticularName { get; set; }
        public string PCREQTN_Purpose { get; set; }
        public string departmentname { get; set; }
        public string PCREQTN_RequisitionNo { get; set; }
        public string employeename { get; set; }
        public string PCREQTNDET_Remarks { get; set; }
        public string message { get; set; }
        public string PCINDENTDET_Remarks { get; set; }
        public decimal? PCINDENT_TotAmount { get; set; }
        public decimal? PCINDENT_ApprovedAmt { get; set; }
        public decimal? PCREQTN_TotAmount { get; set; }
        public decimal? PCINDENT_RequestedAmount { get; set; }        
        public decimal? PCREQTNDET_Amount { get; set; }
        public decimal? PCINDENTDET_Amount { get; set; }
        public decimal? PCINDENTDET_ApprovedAmt { get; set; }
        public bool returnval { get; set; }
        public bool PCINDENT_ActiveFlg { get; set; }
        public bool PCREQTNDET_ActiveFlg { get; set; }
        public bool PCINDENTDET_ActiveFlg { get; set; }
        public Array getloaddata { get; set; }
        public Array getviewdata { get; set; }
        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }
        public string Auto_id { get; set; }
        public Array requisitiondetais { get; set; }
        public Array requisitionparticulardetais { get; set; }
        public temp_requisitionid[] temp_requisitionid { get; set; }
        public PC_Indent_DetailsDTO[] PC_Indent_DetailsDTO { get; set; }
        public Array getapproveddata { get; set; }
        public Array geteditdata { get; set; }
        public Array geteditparticularsdata { get; set; }
        public Array getuserinstitution { get; set; }
        public DateTime? createdate { get; set; }
    }

    public class temp_requisitionid
    {
        public long PCREQTN_Id { get; set; }
    }
    public class PC_Indent_DetailsDTO
    {
        public long PCINDENTDET_Id { get; set; }
        public long PCINDENT_Id { get; set; }
        public long PCREQTN_Id { get; set; }
        public long PCMPART_Id { get; set; }
        public decimal? PCINDENTDET_Amount { get; set; }
        public string PCINDENTDET_Remarks { get; set; }
        public decimal? PCINDENTDET_ApprovedAmt { get; set; }
    }
}
