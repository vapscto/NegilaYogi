using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Documents
{
    public class NaacDocumentUploadDTO
    {
        public long MI_Id { get; set; }
        public long NAACSL_CreatedBy { get; set; }
        public long NAACSL_UpdatedBy { get; set; }
        public long NAACMSL_CreatedBy { get; set; }
        public long NAACMSL_UpdatedBy { get; set; }
        public long NAACSL_Id { get; set; }
        public long NAACSL_ParentId { get; set; }
        public int NAACSL_SLNoOrder { get; set; }
        public Array getzeroarray { get; set; }
        public Array getalldata { get; set; }
        public Array getparentidzero { get; set; }
        public Array getsavedzeroarray { get; set; }
        public Array getsavedparentidzero { get; set; }
        public Array getsavealldata { get; set; }
        public Array savedata { get; set; }
        public string NAACSL_SLNo { get; set; }
        public string NAACSL_SLNoDescription { get; set; }
        public string NAACSL_SLNote { get; set; }
        public string NAASCL_Template { get; set; }
        public string document_Path { get; set; }
        public string file_name { get; set; }
        public string NAACMSL_Status { get; set; }
        public string status { get; set; }
        public string comments { get; set; }
        public string coordinatorcomments { get; set; }
        public string NAACMSL_ConsultantRemarks { get; set; }
        public string NAACMSL_UserRemarks { get; set; }
        public DateTime? NAACMSL_CreatedDate { get; set; }
        public DateTime? NAACMSL_UpdatedDate { get; set; }
        public DateTime? NAACSL_UpdatedDate { get; set; }
        public DateTime? NAACSL_CreatedDate { get; set; }
        public bool NAACMSL_ActiveFlag { get; set; }
        public bool NAACSL_ActiveFlag { get; set; }
        public bool returnval { get; set; }
        public Array getdocumentlist { get; set; }
        public Array getcommentslist { get; set; }
        public Array getgeneralcommentslist { get; set; }
        public bool? NAACSL_TextBoxFlg { get; set; }
        public bool? NAACSL_UploadReq { get; set; }
        public decimal? NAACSL_Percentage { get; set; }
        public decimal percentage { get; set; }
        public string NAACSL_InstitutionTypeFlg { get; set; }
        public string NAACMSLCO_Remarks { get; set; }
        public long NAACMSLF_Id { get; set; }
        public temp_upload_dto[] temp_dto { get; set; }
        public temp_hyperlink_dto[] temp_hyperlink_dto { get; set; }
        public Array viewhyperlinks { get; set; }
        public long NAACMSLLK_Id { get; set; }     
        public string NAACMSLLK_LinkName { get; set; }
        public string NAACMSLLK_LinkRemarks { get; set; }
        public bool NAACMSLLK_ActiveFlag { get; set; }
        public decimal? NAACMSL_CGPA { get; set; }
        public long NAACMSL_Id { get; set; }
        public string userRole { get; set; }

    }

    public class temp_upload_dto
    {
        public string NAACMSLF_FileName { get; set; }
        public string NAACMSLF_FilePath { get; set; }
        public string usercomments { get; set; }
        public string NAACMSLF_UploadDate { get; set; }
        public long NAACSL_Id { get; set; }
        public long NAACMSLF_Id { get; set; }
        public string filestatus { get; set; }
    }
    public class temp_hyperlink_dto
    {
        public long NAACMSLLK_Id { get; set; }
        public long NAACMSL_Id { get; set; }
        public string NAACMSLLK_LinkName { get; set; }
        public string NAACMSLLK_LinkRemarks { get; set; }
    }
}
