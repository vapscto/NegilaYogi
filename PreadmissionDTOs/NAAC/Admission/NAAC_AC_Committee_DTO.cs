using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
    public class NAAC_AC_Committee_DTO
    {
        public long NCACCOMM_Id { get; set; }
        public long MI_Id { get; set; }
        public string NCACCOMM_CommitteeName { get; set; }
        public string NCACCOMM_Flg { get; set; }
        public long NCACCOMM_Year { get; set; }
        public string NCACCOMM_FileName { get; set; }
        public string NCACCOMM_FilePath { get; set; }
        public bool NCACCOMM_ActiveFlg { get; set; }
        public long NCACCOMM_CreatedBy { get; set; }
        public long NCACCOMM_UpdatedBy { get; set; }
        public DateTime NCACCOMM_CreatedDate { get; set; }
        public DateTime NCACCOMM_UpdatedDate { get; set; }
        public string NCACCOMM_StaffFlg { get; set; }
        public long NCACCOMMFC_Id { get; set; }
        public string NCACCOMMFC_Remarks { get; set; }
        public long? NCACCOMMFC_RemarksBy { get; set; }
        public bool? NCACCOMMFC_ActiveFlag { get; set; }
        public long? NCACCOMMFC_CreatedBy { get; set; }
        public DateTime? NCACCOMMFC_CreatedDate { get; set; }
        public long? NCACCOMMFC_UpdatedBy { get; set; }
        public DateTime? NCACCOMMFC_UpdatedDate { get; set; }
        public string NCACCOMMFC_StatusFlg { get; set; }
        public string NCACCOMMF_StatusFlg { get; set; }
        public bool? NCACCOMMF_ActiveFlg { get; set; }
        public bool duplicate { get; set; }
        public long UserId { get; set; }
        public Array yeardata { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public Array yearlist { get; set; }
        public string msg { get; set; }
        public string NCACCOMMM_Role { get; set; }
        public string NCACCOMMM_MemberName { get; set; }
        public string NCACCOMMM_MemberDetails { get; set; }
        public long NCACCOMMM_MemberPhoneNo { get; set; }
        public string NCACCOMMM_MemberEmailId { get; set; }
        public string rdbutton { get; set; }
        public string newb { get; set; }
        public string existing { get; set; }
        public long HRME_Id { get; set; }
        public string all1 { get; set; }
        public Array filldepartment { get; set; }
        public Array institutionlist { get; set; }
        public Array filldesignation { get; set; }
        public long HRMD_Id { get; set; }
        public Array stafftlist { get; set; }
        public long HRMDES_Id { get; set; }
        public string empname { get; set; }
        public Array alldata1 { get; set; }
        public string allorindii { get; set; }
        public selectdStafflist[] selectdStafflist { get; set; }
        public bool ret { get; set; }
        public Array editlist { get; set; }
        public Array ar { get; set; }
        public long NCACCOMMM_Id { get; set; }
        public bool returnval { get; set; }
        public Array mappedstafflist { get; set; }
        public Array viewdocument_MainActUploadFiles { get; set; }
        public string NCACCOMMF_FileName { get; set; }
        public string NCACCOMMF_FileDesc { get; set; }
        public string NCACCOMMF_FilePath { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public Array viewdocument_StaffActUploadFiles { get; set; }
        public long NCACCOMMF_Id { get; set; }
        public long NCACCOMMMF_Id { get; set; }
        public Array editMainSActFileslist { get; set; }
        public NAAC_AC_Committee_DTO[] filelist { get; set; }
        public Array editStaffActFileslist { get; set; }
        public NAAC_AC_Committee_DTO[] filelist_staff { get; set; }
        public long NCACCOMMC_Id { get; set; }
        public string NCACCOMMC_Remarks { get; set; }
        public long? NCACCOMMC_RemarksBy { get; set; }
        public string NCACCOMMMF_StatusFlg { get; set; }
        public bool NCACCOMMMF_ActiveFlg { get; set; }
        public string NCACCOMMC_StatusFlg { get; set; }
        public bool? NCACCOMMC_ActiveFlag { get; set; }
        public long? NCACCOMMC_CreatedBy { get; set; }
        public string NCACCOMMM_StatusFlg { get; set; }
        public DateTime? NCACCOMMC_CreatedDate { get; set; }
        public long? NCACCOMMC_UpdatedBy { get; set; }
        public DateTime? NCACCOMMC_UpdatedDate { get; set; }
        public string NCACCOMM_StatusFlg { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }
        public string UserName { get; set; }
        public Array commentlist1 { get; set; }
        public Array commentlist { get; set; }
        public long NCACCOMMMC_Id { get; set; }
        public string NCACCOMMMC_Remarks { get; set; }
        public long? NCACCOMMMC_RemarksBy { get; set; }
        public string NCACCOMMMC_StatusFlg { get; set; }
        public bool? NCACCOMMMC_ActiveFlag { get; set; }
        public long? NCACCOMMMC_CreatedBy { get; set; }
        public DateTime? NCACCOMMMC_CreatedDate { get; set; }
        public long? NCACCOMMMC_UpdatedBy { get; set; }
        public DateTime? NCACCOMMMC_UpdatedDate { get; set; }
        public Array commentlistmember { get; set; }
        public long NCACCOMMMFC_Id { get; set; }
        public string NCACCOMMMFC_Remarks { get; set; }
        public long? NCACCOMMMFC_RemarksBy { get; set; }
        public bool? NCACCOMMMFC_ActiveFlag { get; set; }
        public long? NCACCOMMMFC_CreatedBy { get; set; }
        public DateTime? NCACCOMMMFC_CreatedDate { get; set; }
        public long? NCACCOMMMFC_UpdatedBy { get; set; }
        public DateTime? NCACCOMMMFC_UpdatedDate { get; set; }
        public string NCACCOMMMFC_StatusFlg { get; set; }
        public Array commentlist1member { get; set; }



        //public long NCACCOMM_Id { get; set; }
        //public long MI_Id { get; set; }
        //public string NCACCOMM_CommitteeName { get; set; }
        //public string NCACCOMM_Flg { get; set; }
        //public long NCACCOMM_Year { get; set; }
        //public string NCACCOMM_FileName { get; set; }
        //public string NCACCOMM_FilePath { get; set; }
        //public bool NCACCOMM_ActiveFlg { get; set; }
        //public long NCACCOMM_CreatedBy { get; set; }
        //public long NCACCOMM_UpdatedBy { get; set; }
        //public DateTime NCACCOMM_CreatedDate { get; set; }
        //public DateTime NCACCOMM_UpdatedDate { get; set; }
        //public string NCACCOMM_StaffFlg { get; set; }
        //public bool duplicate { get; set; }
        //public long UserId { get; set; }
        //public Array yeardata { get; set; }
        //public long ASMAY_Id { get; set; }
        //public string ASMAY_Year { get; set; }
        //public Array yearlist { get; set; }
        //public string msg { get; set; }
        //public string NCACCOMMM_Role { get; set; }
        //public string NCACCOMMM_MemberName { get; set; }
        //public string NCACCOMMM_MemberDetails { get; set; }
        //public long NCACCOMMM_MemberPhoneNo { get; set; }
        //public string NCACCOMMM_MemberEmailId { get; set; }
        //public string rdbutton { get; set; }
        //public string newb { get; set; }
        //public string existing { get; set; }
        //public long HRME_Id { get; set; }
        //public string all1 { get; set; }
        //public Array filldepartment { get; set; }
        //public Array institutionlist { get; set; }
        //public Array filldesignation { get; set; }
        //public long HRMD_Id { get; set; }
        //public Array stafftlist { get; set; }
        //public long HRMDES_Id { get; set; }
        //public string empname { get; set; }
        //public Array alldata1 { get; set; }
        //public string allorindii { get; set; }
        //public selectdStafflist[] selectdStafflist { get; set; }
        //public bool ret { get; set; }
        //public Array editlist { get; set; }
        //public Array ar { get; set; }
        //public long NCACCOMMM_Id { get; set; }
        //public bool returnval { get; set; }
        //public Array mappedstafflist { get; set; }
        //public Array viewdocument_MainActUploadFiles { get; set; }
        //public string NCACCOMMF_FileName { get; set; }
        //public string NCACCOMMF_FileDesc { get; set; }
        //public string NCACCOMMF_FilePath { get; set; }
        //public string cfilename { get; set; }
        //public string cfilepath { get; set; }
        //public string cfiledesc { get; set; }
        //public Array viewdocument_StaffActUploadFiles { get; set; }
        //public long NCACCOMMF_Id { get; set; }
        //public long NCACCOMMMF_Id { get; set; }
        //public Array editMainSActFileslist { get; set; }
        //public Naac_CommonFiles_DTO[] filelist { get; set; }
        //public Array editStaffActFileslist { get; set; }
        //public filelist_staff[] filelist_staff { get; set; }
    }  
    public class selectdStafflist
    {
        public long HRME_Id { get; set; }
    } 
    public class sss
    {
        public long HRME_Id { get; set; }
    }
}
