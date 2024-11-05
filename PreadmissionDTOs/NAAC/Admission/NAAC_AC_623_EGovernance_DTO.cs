using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
    public class NAAC_AC_623_EGovernance_DTO
    {
        public long NCAC623EGOV_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC623EGOV_ImpYear { get; set; }
        public string NCAC623EGOV_GovernanceArea { get; set; }
        public string NCAC623EGOV_VendorName { get; set; }
        public string NCAC623EGOV_VendorAddress { get; set; }
        public long NCAC623EGOV_VendorPhoneNo { get; set; }
        public string NCAC623EGOV_VendorEmailId { get; set; }
        public string NCAC623EGOVF_Filedesc { get; set; }
        public string NCAC623EGOVF_FileName { get; set; }
        public string NCAC623EGOVF_FilePath { get; set; }
        public bool NCAC623EGOV_ActiveFlg { get; set; }
        public long NCAC623EGOV_CreatedBy { get; set; }
        public long NCAC623EGOV_UpdatedBy { get; set; }
        public DateTime NCAC623EGOV_CreatedDate { get; set; }
        public DateTime NCAC623EGOV_UpdatedDate { get; set; }
        public long UserId { get; set; }
        public Array allacademicyear { get; set; }
        public Array institutionlist { get; set; }
        public Array alldata1 { get; set; }
        public string ASMAY_Year { get; set; }
        public long ASMAY_Id { get; set; }
        public bool duplicate {get;set;}
        public bool returnval { get;set;}
        public string msg { get;set;}
        public Array editlist { get;set;}

        public long NCAC623EGOVF_Id { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public Array viewuploadflies { get; set; }
        public Array editFileslist { get; set; }
        public NAAC_AC_623_EGovernance_DTO[] filelist { get; set; }

        public string NCAC623EGOVF_StatusFlg { get; set; }
        public bool? NCAC623EGOVF_ActiveFlg { get; set; }
        public bool? NCAC623EGOVF_ApprovedFlg { get; set; }
        public string NCAC623EGOVF_Remarks { get; set; }


        public string NCAC623EGOV_StatusFlg { get; set; }
        public bool? NCAC623EGOV_ApprovedFlg { get; set; }
        public string NCAC623EGOV_Remarks { get; set; }

        public long NCAC623EGOVC_Id { get; set; }
        public string NCAC623EGOVC_Remarks { get; set; }
        public long? NCAC623EGOVC_RemarksBy { get; set; }
        public string NCAC623EGOVC_StatusFlg { get; set; }
        public bool? NCAC623EGOVC_ActiveFlag { get; set; }
        public long? NCAC623EGOVC_CreatedBy { get; set; }
        public DateTime? NCAC623EGOVC_CreatedDate { get; set; }
        public long? NCAC623EGOVC_UpdatedBy { get; set; }
        public DateTime? NCAC623EGOVC_UpdatedDate { get; set; }
        public long NCAC623EGOVFC_Id { get; set; }
        public string NCAC623EGOVFC_Remarks { get; set; }
        public long? NCAC623EGOVFC_RemarksBy { get; set; }
        public bool? NCAC623EGOVFC_ActiveFlag { get; set; }
        public long? NCAC623EGOVFC_CreatedBy { get; set; }
        public DateTime? NCAC623EGOVFC_CreatedDate { get; set; }
        public long? NCAC623EGOVFC_UpdatedBy { get; set; }
        public DateTime? NCAC623EGOVFC_UpdatedDate { get; set; }
        public string NCAC623EGOVFC_StatusFlg { get; set; }
        public string UserName { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }
        public long cfileid { get; set; }

        public Array commentlist { get; set; }
        public Array commentlist1 { get; set; }


    }
}
