using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Portals.Student
{
    public class TransferCertificate_DTO : CommonParamDTO
    {

        public long ASCA_Id { get; set; }

        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
      
        public long ASMAY_Id { get; set; }
        public string AMCT_Certificate_code { get; set; }
        public string AMCT_Certificate_Name { get; set; }
        public string ASCA_CertificateType { get; set; }
        public string ASCA_Reason { get; set; }
        public string employeename { get; set; }
        public string ATCCTAPP_Remarks { get; set; }
        public string ATCLIBAPP_Remarks { get; set; }
        public string ATCFAPP_Remarks { get; set; }
        public string ATCPDAAPP_Remarks { get; set; }
        public string message { get; set; }
        public DateTime ATCCTAPP_ApprovedDate { get; set; }
        public DateTime ATCLIBAPP_ApprovedDate { get; set; }
        public DateTime ATCFAPP_ApprovedDate { get; set; }
        public DateTime ATCPDAAPP_ApprovedDate { get; set; }
        public DateTime ASCA_ApplyDate { get; set; }
        public string ASCA_Status { get; set; }
        public bool ASCA_ActiveFlg { get; set; }


        public long ASCAP_Id { get; set; }
        public long IVRMALU_Id { get; set; }
        public string ASCAP_Status { get; set; }
        public string ASCAP_ApproveReason { get; set; }
        public DateTime ASCAP_ApproveDate { get; set; }
        public bool ASCAP_ActiveFlg { get; set; }
        public bool ACERTAPP_ActiveFlg { get; set; }


        public long ASMCL_Id { get; set; }
        public string AMST_FirstName { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public long ASMS_Id { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public string AMST_emailId { get; set; }
        public long AMST_MobileNo { get; set; }


        public Array ct_approval { get; set; }
        public Array library_approval { get; set; }
        public Array fee_approval { get; set; }
        public Array pda_approval { get; set; }


        public Array certificate_dropdown { get; set; }
        public Array certificatelist { get; set; }

        public Array studentdetails { get; set; }
        public Array aply_aprvlist { get; set; }
        public Array editdata { get; set; }
        public string AMST_AdmNo { get; set; }
        public string ASMAY_Year { get; set; }
        public string flag { get; set; }
        public long UserId { get; set; }
        public long IVRMRT_Id { get; set; }
        public string flag_Type { get; set; }
        public long ACERTAPP_Id { get; set; }
        public string ACERTAPP_CertificateName { get; set; }
        public string ACERTAPP_CertificateCode { get; set; }
        public bool ACERTAPP_OnlineDownloadFlg { get; set; }
        public bool ACERTAPP_ApprovaReqlFlg { get; set; }
        public string AMCT_Description { get; set; }
        public bool AMCT_ActiceFlag { get; set; }
        public Array get_certificate { get; set; }
        public Array get_details { get; set; }
        public Array get_certificate_dd { get; set; }

        public Array totalcount { get; set; }

        public bool returnval { get; set; }
        public string returnvalues { get; set; }
        public string roletype { get; set; }
        public long HRME_Id { get; set; }
        public Array studlist { get; set; }
        public Array applylist { get; set; }
        public bool duplicate { get; set; }
        public Array student_tc_list { get; set; }
        public student_id_list1[] student_id_list { get; set; }

        public class student_id_list1
        {
            public long AMST_Id { get; set; }
        }

    }
}
