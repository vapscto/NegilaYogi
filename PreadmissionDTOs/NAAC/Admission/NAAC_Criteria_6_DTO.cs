using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
    public class NAAC_Criteria_6_DTO
    {

        public long cycleid { get; set; }
        public Array getinstitutioncycle { get; set; }
        public Array getinstitution { get; set; }

        public long NCAC632FINSUP_Id { get; set; }
        public long NCAC653IQAC_NoOfTeacher { get; set; }
        public string NCAC653IQAC_Venue { get; set; }
        public string returnvaledit { get; set; }
        public string NCAC642FUND_GovORNongovFlag { get; set; }
        public string flagradio { get; set; }
        public long NCAC623EGOV_Id { get; set; }
        public long NCAC633ADMTRG_Id { get; set; }
        public long NCAC634DEVPRG_Id { get; set; }
        public long NCAC642FUND_Id { get; set; }
        public long NCAC653IQAC_Id { get; set; }
        public long NCAC654QUAS_Id { get; set; }

        public long NCAC633ADMTRGF_Id { get; set; }
        public long NCAC634DEVPRGF_Id { get; set; }
        public long NCAC642FUNDF_Id { get; set; }
        public long NCAC653IQACF_Id { get; set; }
        public long NCAC654QUASF_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string Name { get; set; }
        public string org { get; set; }
        public long duration { get; set; }
        public decimal amount { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
        public bool flag1 { get; set; }
        public bool flag2 { get; set; }
        public bool flag3 { get; set; }
        public bool flag4 { get; set; }
        public bool flag5 { get; set; }
        public bool flag51 { get; set; }
        public bool flag52 { get; set; }
        public bool flag6 { get; set; }
        public long TotalCount { get; set; }
        public string description { get; set; }
        public string FileName { get; set; }
        public string filepath { get; set; }
        public long createdby { get; set; }
        public long updatedby { get; set; }
        public Array savedresult { get; set; }
        public Array allacademicyear { get; set; }
        public Array alldata1 { get; set; }
        public string ASMAY_Year { get; set; }
        public string msg { get; set; }
        public string flag7 { get; set; }
        public long UserId { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public bool ret { get; set; }
        public Array editlist { get; set; }
        public Array institutionlist { get; set; }
        public Array editfiles { get; set; }

        public pgTempDTO[] pgTempDTO { get; set; }
        public string LPMTR_Resources { get; set; }
        public string file_name { get; set; }
        public string filetype { get; set; }
        public string desc { get; set; }

        public string fdate { get; set; }
        public string tdate { get; set; }
        public NAAC_Criteria_6_DTO[] yerlistdata { get; set; }

        public bool NCAC653IQAC_RegIQACFlg { get; set; }
        public bool NCAC653IQAC_FeedbackClgImprts { get; set; }
        public bool NCAC653IQAC_PrepOfDocAccBodiesFlg { get; set; }
        public string NCAC634DEVPRG_NameOfTeachers { get; set; }
        public string Remarks { get; set; }
        public string UserName { get; set; }
        public long filefkid { get; set; }

        public long cfileid { get; set; }
        public long NCAC633ADMTRGC_Id { get; set; }
        public string NCAC633ADMTRGC_Remarks { get; set; }
        public long? NCAC633ADMTRGC_RemarksBy { get; set; }
        public string NCAC633ADMTRGC_StatusFlg { get; set; }
        public bool? NCAC633ADMTRGC_ActiveFlag { get; set; }
        public long NCAC633ADMTRGC_CreatedBy { get; set; }
        public DateTime? NCAC633ADMTRGC_CreatedDate { get; set; }
        public long? NCAC633ADMTRGC_UpdatedBy { get; set; }
        public DateTime? NCAC633ADMTRGC_UpdatedDate { get; set; }


        public long NCAC634DEVPRGC_Id { get; set; }
        public string NCAC634DEVPRGC_Remarks { get; set; }
        public long? NCAC634DEVPRGC_RemarksBy { get; set; }
        public string NCAC634DEVPRGC_StatusFlg { get; set; }
        public bool? NCAC634DEVPRGC_ActiveFlag { get; set; }
        public long? NCAC634DEVPRGC_CreatedBy { get; set; }
        public DateTime? NCAC634DEVPRGC_CreatedDate { get; set; }
        public long? NCAC634DEVPRGC_UpdatedBy { get; set; }
        public DateTime? NCAC634DEVPRGC_UpdatedDate { get; set; }


        public long NCAC634DEVPRGFC_Id { get; set; }
        public string NCAC634DEVPRGFC_Remarks { get; set; }
        public long? NCAC634DEVPRGFC_RemarksBy { get; set; }
        public bool? NCAC634DEVPRGFC_ActiveFlag { get; set; }
        public long? NCAC634DEVPRGFC_CreatedBy { get; set; }
        public DateTime? NCAC634DEVPRGFC_CreatedDate { get; set; }
        public long? NCAC634DEVPRGFC_UpdatedBy { get; set; }
        public DateTime? NCAC634DEVPRGFC_UpdatedDate { get; set; }
        public string NCAC634DEVPRGFC_StatusFlg { get; set; }

        public long NCAC642FUNDC_Id { get; set; }
        public string NCAC642FUNDC_Remarks { get; set; }
        public long? NCAC642FUNDC_RemarksBy { get; set; }
        public string NCAC642FUNDC_StatusFlg { get; set; }
        public bool? NCAC642FUNDC_ActiveFlag { get; set; }
        public long? NCAC642FUNDC_CreatedBy { get; set; }
        public DateTime? NCAC642FUNDC_CreatedDate { get; set; }
        public long? NCAC642FUNDC_UpdatedBy { get; set; }
        public DateTime? NCAC642FUNDC_UpdatedDate { get; set; }

        public string NCAC642FUND_StatusFlg { get; set; }
        public bool? NCAC642FUND_ApprovedFlg { get; set; }
        public string NCAC642FUND_Remarks { get; set; }

        public long NCAC642FUNDFC_Id { get; set; }
        public string NCAC642FUNDFC_Remarks { get; set; }
        public long? NCAC642FUNDFC_RemarksBy { get; set; }
        public bool? NCAC642FUNDFC_ActiveFlag { get; set; }
        public long? NCAC642FUNDFC_CreatedBy { get; set; }
        public DateTime? NCAC642FUNDFC_CreatedDate { get; set; }
        public long? NCAC642FUNDFC_UpdatedBy { get; set; }
        public DateTime? NCAC642FUNDFC_UpdatedDate { get; set; }
        public string NCAC642FUNDFC_StatusFlg { get; set; }

        public string NCAC642FUNDF_StatusFlg { get; set; }
        public bool? NCAC642FUNDF_ActiveFlg { get; set; }
        public bool? NCAC642FUNDF_ApprovedFlg { get; set; }
        public string NCAC642FUNDF_Remarks { get; set; }

        public long NCAC633ADMTRGFC_Id { get; set; }
        public string NCAC633ADMTRGFC_Remarks { get; set; }
        public long? NCAC633ADMTRGFC_RemarksBy { get; set; }
        public bool? NCAC633ADMTRGFC_ActiveFlag { get; set; }
        public long? NCAC633ADMTRGFC_CreatedBy { get; set; }
        public DateTime? NCAC633ADMTRGFC_CreatedDate { get; set; }
        public long? NCAC633ADMTRGFC_UpdatedBy { get; set; }
        public DateTime? NCAC633ADMTRGFC_UpdatedDate { get; set; }
        public string NCAC633ADMTRGFC_StatusFlg { get; set; }

        public string NCAC633ADMTRGF_StatusFlg { get; set; }
        public bool? NCAC633ADMTRGF_ApprovedFlg { get; set; }
        public string NCAC633ADMTRGF_Remarks { get; set; }

        public string NCAC633ADMTRG_StatusFlg { get; set; }
        public bool? NCAC633ADMTRG_ApprovedFlg { get; set; }
        public string NCAC633ADMTRG_Remarks { get; set; }

        public Array commentlist { get; set; }
        public Array commentlist1 { get; set; }

        public string NCAC634DEVPRGF_StatusFlg { get; set; }
        public bool? NCAC634DEVPRGF_ActiveFlg { get; set; }
        public bool? NCAC634DEVPRGF_ApprovedFlg { get; set; }
        public string NCAC634DEVPRGF_Remarks { get; set; }

        public string NCAC634DEVPRG_StatusFlg { get; set; }
        public bool? NCAC634DEVPRG_ApprovedFlg { get; set; }
        public string NCAC634DEVPRG_Remarks { get; set; }

        public long NCAC654QUASC_Id { get; set; }
        public string NCAC654QUASC_Remarks { get; set; }
        public long? NCAC654QUASC_RemarksBy { get; set; }
        public string NCAC654QUASC_StatusFlg { get; set; }
        public bool? NCAC654QUASC_ActiveFlag { get; set; }
        public long? NCAC654QUASC_CreatedBy { get; set; }
        public DateTime? NCAC654QUASC_CreatedDate { get; set; }
        public long? NCAC654QUASC_UpdatedBy { get; set; }
        public DateTime? NCAC654QUASC_UpdatedDate { get; set; }



        public long NCAC654QUASFC_Id { get; set; }
        public string NCAC654QUASFC_Remarks { get; set; }
        public long? NCAC654QUASFC_RemarksBy { get; set; }
        public bool? NCAC654QUASFC_ActiveFlag { get; set; }
        public long? NCAC654QUASFC_CreatedBy { get; set; }
        public DateTime? NCAC654QUASFC_CreatedDate { get; set; }
        public long? NCAC654QUASFC_UpdatedBy { get; set; }
        public DateTime? NCAC654QUASFC_UpdatedDate { get; set; }
        public string NCAC654QUASFC_StatusFlg { get; set; }

        public string NCAC654QUAS_StatusFlg { get; set; }
        public bool? NCAC654QUAS_ApprovedFlg { get; set; }
        public string NCAC654QUAS_Remarks { get; set; }

        public string NCAC654QUASF_StatusFlg { get; set; }
        public bool? NCAC653IQACF_ActiveFlg { get; set; }
        public bool? NCAC654QUASF_ApprovedFlg { get; set; }
        public string NCAC654QUASF_Remarks { get; set; }


        public string NCAC653IQAC_StatusFlg { get; set; }
        public bool? NCAC653IQAC_ApprovedFlg { get; set; }
        public string NCAC653IQAC_Remarks { get; set; }


        public string NCAC653IQACF_StatusFlg { get; set; }
        public bool? NCAC653IQACF_ApprovedFlg { get; set; }
        public string NCAC653IQACF_Remarks { get; set; }


        public long NCAC653IQACC_Id { get; set; }
        public string NCAC653IQACC_Remarks { get; set; }
        public long? NCAC653IQACC_RemarksBy { get; set; }
        public string NCAC653IQACC_StatusFlg { get; set; }
        public bool? NCAC653IQACC_ActiveFlag { get; set; }
        public long? NCAC653IQACC_CreatedBy { get; set; }
        public DateTime? NCAC653IQACC_CreatedDate { get; set; }
        public long? NCAC653IQACC_UpdatedBy { get; set; }
        public DateTime? NCAC653IQACC_UpdatedDate { get; set; }

        public long NCAC653IQACFC_Id { get; set; }
        public string NCAC653IQACFC_Remarks { get; set; }
        public long? NCAC653IQACFC_RemarksBy { get; set; }
        public bool? NCAC653IQACFC_ActiveFlag { get; set; }
        public long? NCAC653IQACFC_CreatedBy { get; set; }
        public DateTime? NCAC653IQACFC_CreatedDate { get; set; }
        public long? NCAC653IQACFC_UpdatedBy { get; set; }
        public DateTime? NCAC653IQACFC_UpdatedDate { get; set; }
        public string NCAC653IQACFC_StatusFlg { get; set; }

    }
    public class pgTempDTO
    {
        public long PRYR_Id { get; set; }
        public string LPMTR_Resources { get; set; }
        public string file_name { get; set; }
        public string filetype { get; set; }
        public string desc { get; set; }
        public long cfileid { get; set; }
    }
}
