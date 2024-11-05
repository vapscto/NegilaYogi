using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager.PettyCash
{
    public class PC_RequisitionDTO
    {
        public long MI_Id { get; set; }
        public long Userid { get; set; }
        public long roleid { get; set; }
        public long HRMD_Id { get; set; }
        public long PCREQTN_Id { get; set; }
        public long HRME_Id { get; set; }
        public long pcreqtndeT_Id { get; set; }
        public long PCMPART_Id { get; set; }
        public decimal? amount { get; set; }
        public string remarks { get; set; }
        public string Role_flag { get; set; }
        public Array getdept { get; set; }
        public Array getemp { get; set; }
        public Array getparticulars { get; set; }
        public Array documentdetails { get; set; }
        public string empname { get; set; }
        public string deptname { get; set; }
        public string PCREQTN_Purpose { get; set; }
        public string message { get; set; }
        public string saveorupdate { get; set; }
        public decimal? PCREQTN_TotAmount { get; set; }
        public DateTime? PCREQTN_Date { get; set; }
        public DateTime? PCREQTN_CreatedDate { get; set; }
        public bool returnval { get; set; }
        public PC_Requisition_DetailsDTO[] PC_Requisition_DetailsDTO { get; set; }
        public Array geteditdept { get; set; }
        public Array geteditdetails { get; set; }
        public Array geteditemp { get; set; }
        public Array geteditsavedemp { get; set; }
        public Array geteditsavedparticulars { get; set; }
        public string PCMPART_ParticularName { get; set; }
        public decimal? PCREQTNDET_Amount { get; set; }
        public string PCREQTNDET_Remarks { get; set; }
        public bool PCREQTNDET_ActiveFlg { get; set; }
        public bool PCREQTN_ActiveFlg { get; set; }
        public Array getviewdata { get; set; }
        public Array getloaddata { get; set; }
        public string departmentname { get; set; }
        public string employeename { get; set; }
        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }
        public string Auto_id { get; set; }
        public string PCREQTN_RequisitionNo { get; set; }
        public long ASMAY_Id { get; set; }
        public Array getapprovedindent { get; set; }
        public uploaddocments[] uploaddocments { get; set; }
    }

    public class PC_Requisition_DetailsDTO
    {
        public long PCMPART_Id { get; set; }
        public long PCREQTNDET_Id { get; set; }
        public string PCMPART_ParticularName { get; set; }
        public decimal? PCREQTNDET_Amount { get; set; }
        public string PCREQTNDET_Remarks { get; set; }
    }

    public class uploaddocments
    {
        public string PCREQTNUP_Id { get; set; }
        public string PCREQTNUP_FileName { get; set; }
        public string PCREQTNUP_FileLocation { get; set; }
    }
}

