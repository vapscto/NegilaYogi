using DomainModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("HR_Candidate_Details")]
    public class HR_Candidate_DetailsDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRCD_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMPT_Id { get; set; }
        public long HRMC_Id { get; set; }
        public long HRCD_MRFNO { get; set; }
        public string HRCD_FirstName { get; set; }
        public string HRCD_MiddleName { get; set; }
        public string HRCD_LastName { get; set; }
        public long HRMJ_Id { get; set; }
        //public string HRCD_JobTitle { get; set; }
        public string HRCD_Skills { get; set; }
        public DateTime HRCD_DOB { get; set; }
        public long IVRMMG_Id { get; set; }
        public long HRCD_MobileNo { get; set; }
        public string HRCD_EmailId { get; set; }
        public decimal HRCD_ExpFrom { get; set; }
        public decimal HRCD_ExpTo { get; set; }
        public string HRCD_CurrentCompany { get; set; }
        public string HRCD_ResumeSource { get; set; }
        public string HRCD_JobPortalName { get; set; }
        public string HRCD_RefCode { get; set; }
        //public long HRCD_RefCode { get; set; }
        public decimal HRCD_LastCTC { get; set; }
        public decimal HRCD_ExpectedCTC { get; set; }
        public DateTime HRCD_AppDate { get; set; }
        public DateTime HRCD_InterviewDate { get; set; }
        public long HRCD_NoticePeriod { get; set; }
        public string HRCD_Remarks { get; set; }
        public string HRCD_Resume { get; set; }
        public string HRCD_RecruitmentStatus { get; set; }
        public bool HRCD_ActiveFlg { get; set; }
        public long HRCD_CreatedBy { get; set; }
        public long HRCD_UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string HRCD_Photo { get; set; }
        public string HRCD_FatherName { get; set; }
        public long? HRCD_AadharNo { get; set; }
        public string HRCD_PAN { get; set; }
        public DateTime? HRCD_JoiningDate { get; set; }
        public string HRCD_Designation { get; set; }
        public long? HRCD_BondDuration { get; set; }
        public string HRCD_AddressLocal { get; set; }
        public string HRCD_AddressPermanent { get; set; }
        public string HRCD_NatureOfWork { get; set; }
        public string HRCD_ScopeOfService { get; set; }
        public string HRCD_SHName { get; set; }
        public string HRCD_SHAddress { get; set; }
        public long? HRCD_SHGender { get; set; }
        public long? HRCD_SHContactNo { get; set; }
        public string HRCD_Place { get; set; }
        public long? HRCD_PINCode { get; set; }
        public string HRCD_Department { get; set; }
        public long HRCD_NationalityId { get; set; }
        public long HRCD_Religion { get; set; }
        public long HRCD_MaritalStatus { get; set; }
        public string HRCD_BloodGroup { get; set; }
        public long HRCD_CasteId { get; set; }

        public string HRCD_AddressLocal2 { get; set; }
        public string HRCD_AddLocalPlace { get; set; }
        public long HRCD_AddLocalPIN { get; set; }
        public string HRCD_AddressPermanent2 { get; set; }
        public string HRCD_AddPermanentPlace { get; set; }
        public long HRCD_AddPermanentPIN { get; set; }
        public string HRCD_SHAddress2 { get; set; }

    }
}