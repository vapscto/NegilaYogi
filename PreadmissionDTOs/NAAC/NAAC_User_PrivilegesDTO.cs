using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC
{
    public class NAAC_User_PrivilegesDTO
    {
        public long MI_Id { get; set; }
        public long HRME_Id{get;set;}
        public long NAACSL_Id { get; set; }
        public long userid { get; set; }
        public long NAACUPRI_Id { get; set; }
        public long NAACUPRISL_Id { get; set; }
        public long NAACUPRIIN_Id { get; set; }
        public int NAACUPRI_Order { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string NAACSL_SLNo { get; set; }
        public string NAACSL_SLNoDescription { get; set; }
        public string MI_Name { get; set; }
        public Array getemployee { get; set; }
        public Array getcriteria { get; set; }
        public Array getinstitution { get; set; }
        public Array getsavedata { get; set; }
        public Array getsavedinstitution { get; set; }
        public Array getusersaveddetails { get; set; }
        public bool NAACUPRI_AddFlg { get; set; }
        public bool NAACUPRI_UpdateFlg { get; set; }
        public bool NAACUPRI_DeleteFlg { get; set; }
        public bool NAACUPRI_TrustUserFlag { get; set; }
        public bool NAACUPRI_IQACInchargeFlg { get; set; }
        public bool NAACUPRI_ConsultantFlg { get; set; }
        public bool? NAACUPRI_ApproverFlg { get; set; }
        public bool NAACUPRI_ActiveFlag { get; set; }
        public bool NAACUPRI_FinalFlg { get; set; }
        public bool NAACUPRIIN_ActiveFlag { get; set; }
        public bool returnval { get; set; }
        public bool NAACUPRISL_ActiveFlag { get; set; }
        public temp_miid[] temp_miid { get; set; }
        public int flag { get; set; }
        public Array getsavedcriteria { get; set; }
        public Array getsavedinstituiton { get; set; }
        public Array getalldata { get; set; }

        public NAAC_User_PrivilegesDTO[] mainheaderlist  { get; set; }
        public NAAC_User_PrivilegesDTO[] headerlist { get; set; }
        public NAAC_User_PrivilegesDTO[] subheaderlist { get; set; }
        public string TabFlag { get; set; }
        public string criterianame { get; set; }
        public string returnmessage { get; set; }
        public string NAACSL_InstitutionTypeFlg { get; set; }
        public string NAASCL_Template { get; set; }
        public bool? NAACSL_TextBoxFlg { get; set; }
        public bool? NAACSL_UploadReq { get; set; }
        public decimal? NAACSL_Percentage { get; set; }
        public int NAACSL_SLNoOrder { get; set; }
        public int checkorder { get; set; }
        public long ParentId { get; set; }
        public Array GetZeroParentIdDetails { get; set; }
        public Array GetZeroParentIdOrderDetails { get; set; }
        public Array GetEditTabOneDetails { get; set; }
        public Array GetSavedZeroPatentIdDetails { get; set; }
        public Array GetTabTwoData { get; set; }
        public Array GetTabTwoDataOrder { get; set; }
        public Array GetEditTabTwoDetails { get; set; }
        public Array GetEditTabThreeDetails { get; set; }
        public Array GetSavedPatentIdDetails { get; set; }
        public Array GetTabThreeData { get; set; }
        public string NAACSL_SLNote { get; set; }
    }
    public class temp_miid
    {
        public long MI_Id { get; set; }
    }
}
