using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Portals.Employee
{
    public class Adm_TC_Approval_DTO
    {
        public long ATCFAPP_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long AMST_Id { get; set; }
        public DateTime ATCFAPP_ApprovedDate { get; set; }
        public long ATCFAPP_ApprovedBy { get; set; }
        public long ATCFAPP_FeeGroupId { get; set; }
        public bool ATCFAPP_ApprovalFlg { get; set; }
        public string ATCFAPP_Remarks { get; set; }
        public bool ATCFAPP_ActiveFlg { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ATCFAPP_CreatedBy { get; set; }
        public long ATCFAPP_UpdatedBy { get; set; }
        public long ATCCTAPP_Id { get; set; }
        public DateTime ATCCTAPP_ApprovedDate { get; set; }
        public long ATCCTAPP_ApprovedBy { get; set; }
        public bool ATCCTAPP_AttendanceApprovalFlg { get; set; }
        public bool ATCCTAPP_ExamApprovalFlg { get; set; }
        public string ATCCTAPP_Remarks { get; set; }
        public bool ATCCTAPP_ActiveFlg { get; set; }
        public long ATCCTAPP_CreatedBy { get; set; }
        public long ATCCTAPP_UpdatedBy { get; set; }
        public long ATCLIBAPP_Id { get; set; }
        public DateTime ATCLIBAPP_ApprovedDate { get; set; }
        public long ATCLIBAPP_ApprovedBy { get; set; }
        public bool ATCLIBAPP_ApprovalFlg { get; set; }
        public string ATCLIBAPP_Remarks { get; set; }
        public bool ATCLIBAPP_ActiveFlg { get; set; }
        public long ATCLIBAPP_CreatedBy { get; set; }
        public long ATCLIBAPP_UpdatedBy { get; set; }
        public long ACERTAPP_Id { get; set; }
        public string ACERTAPP_CertificateName { get; set; }
        public long ACERTAPP_CertificateCode { get; set; }
        public bool ACERTAPP_ApprovaReqlFlg { get; set; }
        public bool ACERTAPP_OnlineDownloadFlg { get; set; }
        public bool ACERTAPP_ActiveFlg { get; set; }
        public long ACERTAPP_CreatedBy { get; set; }
        public long ACERTAPP_UpdatedBy { get; set; }
        public string returndata { get; set; }
        public string APPFLG { get; set; }
        public long ATCPDAAPP_Id { get; set; }
        public DateTime ATCPDAAPP_ApprovedDate { get; set; }
        public bool ATCPDAAPP_ApprovalFlg { get; set; }
        public string ATCPDAAPP_Remarks { get; set; }

        public Array student_dd { get; set; }
        public Array tc_ct_list { get; set; }
        public Array tc_fda_list { get; set; }
        public Array tc_library_list { get; set; }
        public Array tc_fee_list { get; set; }
        public Array tc_ct_details { get; set; }
        public Array tc_fee_details { get; set; }
        public Array feehead_details { get; set; }
        public Array tc_library_details { get; set; }
        public Array libstudetails { get; set; }
        public Array exmdetails { get; set; }
        public feeblcarray1[] feeblcarray { get; set; }


        public class feeblcarray1
        {
            public long FMG_Id { get; set; }
        }
    }
}
