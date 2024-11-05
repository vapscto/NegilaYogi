using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.ClubManagement
{
   public class CMS_MastermemberDTO
    {
        public long CMSMMEM_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long CMSMAPPL_Id { get; set; }
        public Array pages { get; set; }

        public long CMSMCAT_Id { get; set; }
        public string CMSMMEM_Proposedby { get; set; }
        public string CMSMMEM_MemberFirstName { get; set; }
        public string CMSMMEM_MemberMiddleName { get; set; }
        public string CMSMMEM_MemberLastName { get; set; }
        public string CMSMMEM_MembershipNo { get; set; }
        public string CMSMMEM_BiometricCode { get; set; }
        public string CMSMMEM_RFCardId { get; set; }
        public string CMSMMEM_PerAdd1 { get; set; }
        public string CMSMMEM_PerAdd2 { get; set; }
        public string CMSMMEM_PerAdd3 { get; set; }
        public string CMSMMEM_PerAdd4 { get; set; }
    
        public long CMSMMEM_PerState { get; set; }

       
        public long CMSMMEM_PerCountry { get; set; }
        public long CMSMMEM_PerPincode { get; set; }

        public string CMSMMEM_LacAdd2 { get; set; }
        public string CMSMMEM_LocAdd1 { get; set; }
        public string CMSMMEM_LocAdd3 { get; set; }

        public string CMSMMEM_LocAdd4 { get; set; }
 
        public long CMSMMEM_LocState { get; set; }
     

        public long CMSMMEM_LocCountry { get; set; }
        public long CMSMMEM_LocPincode { get; set; }

        public long IVRMMMS_Id { get; set; }
       
        public long IMCC_Id { get; set; }
    
        public long IVRMMG_Id { get; set; }
     
        public long IMC_Id { get; set; }
        public string CMSMMEM_SpouseName { get; set; }
        public string CMSMMEM_MotherName { get; set; }
        public string CMSMMEM_FatherName { get; set; }

       
        public long IVRMMR_Id { get; set; }
        public string CMSMMEM_SpouseOccupation { get; set; }
        public long CMSMMEM_UpdatedBy { get; set; }
        public DateTime? CMSMMEM_UpdatedDate { get; set; }

        public long CMSTRANSMEM_UpdatedBy { get; set; }
        public DateTime? CMSTRANSMEM_UpdatedDate { get; set; }
        public long CMSTRANSMEM_Id { get; set; }

        public long CMSTRANS_Id { get; set; }


        public long CMSMMEM_CreatedBy { get; set; }
        public DateTime? CMSMMEM_CreatedDate { get; set; }
        public bool CMSMMEM_ActiveFlag { get; set; }
        public string CMSMMEM_LeavingReason { get; set; }
        public DateTime? CMSMMEM_DOL { get; set; }


        public bool CMSMMEM_LeftFlag { get; set; }
        public DateTime? CMSMMEM_TerminatedDate { get; set; }
        public string CMSMMEM_TerminatedReason { get; set; }
        public bool CMSMMEM_TerminatedFlg { get; set; }
        public bool CMSMMEM_BlockedFlg { get; set; }
        public bool CMSMMEM_OtherClubMemberFlg { get; set; }
        public DateTime? CMSMMEM_MembershipExpDate { get; set; }
        public string CMSMMEM_UINo { get; set; }
        public string CMSMMEM_NationalSSN { get; set; }
        public string CMSMMEM_AadharCardNo { get; set; }
        public string CMSMMEM_PANCardNo { get; set; }
        public string CMSMMEM_ApprovedOn { get; set; }
        public string CMSMMEM_ApproverNo { get; set; }
        public string CMSMMEM_IdentificationMark { get; set; }
        public string CMSMMEM_EyeSightIssue { get; set; }
        public string CMSMMEM_AnyHealthIssue { get; set; }
        public decimal CMSMMEM_WeightUOM { get; set; }
        public decimal CMSMMEM_Weight { get; set; }

        public string CMSMMEM_HeightUOM { get; set; }
        public string CMSMMEM_Height { get; set; }
        public string CMSMMEM_Photo { get; set; }

        public string CMSMMEM_BloodGroup { get; set; }
        public DateTime? CMSMMEM_DOB { get; set; }
        public string CMSMMEM_SpouseAddress { get; set; }
        public string CMSMMEM_SpouseEmailId { get; set; }

        public long CMSMMEM_SpouseMobileNo { get; set; }
        public string returnval { get; set; }
        public Array getreport { get; set; }
        public Array catlist { get; set; }
        public Array memlist { get; set; }

        public string CMSMCAT_CategoryName { get; set; }
        public string CMSMAPPL_ApplicantsName { get; set; }
        public Array allcountry { get; set; }
        public Array allstate { get; set; }
        public Array gender { get; set; }
        public Array mastercaste { get; set; }
        public Array castecategory { get; set; }
        public Array allReligion { get; set; }
        public Array editmember { get; set; }
        public Array AccdemiYear { get; set; }
        public Array editqulify { get; set; }
        public Array editexp { get; set; }
        public Array editnumber { get; set; }
        public Array editemail { get; set; }
        public Array editdocument { get; set; }
    }
}
