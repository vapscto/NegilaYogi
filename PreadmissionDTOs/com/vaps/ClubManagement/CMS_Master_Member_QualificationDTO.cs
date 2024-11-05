using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.ClubManagement
{
  public  class CMS_Master_Member_QualificationDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long CMSMMEMQULQ_Id { get; set; }
        public long CMSMMEM_Id { get; set; }
        public string CMSMMEMQUL_QualificationName { get; set; }
        public string CMSMMEMQULQ_CollegeName { get; set; }
        public string CMSMMEMQULQ_UniversityName { get; set; }
        public string CMSMMEMQULQ_YearOfPassing { get; set; }
        public long CMSMMEMQULQ_State { get; set; }
        public long CMSMMEMQULQ_Country { get; set; }
        public string CMSMMEMQULQ_RegistrationNo { get; set; }
        public string CMSMMEMQULQ_Result { get; set; }
        public decimal CMSMMEMQULQ_CGPAOrPerFlag { get; set; }
        public bool CMSMMEMQULQ_PHDFlg { get; set; }
        public string CMSMMEMQULQ_ThesisTitle { get; set; }
        public long CMSMMEMQULQ_RegistrationYear { get; set; }
        public string CMSMMEMQULQ_GuideName { get; set; }
        public string CMSMMEMQULQ_CGPA { get; set; }
        public decimal CMSMMEMQULQ_Percentage { get; set; }
        public string CMSMMEMQULQ_AreaOfSpecialisation { get; set; }
        public string CMSMMEMQULQ_MedicalCouncil { get; set; }
        public DateTime? CMSMMEMQULQ_Date { get; set; }
        public string CMSMMEMQULQ_Hardcopy { get; set; }
        public bool CMSMMEMQULQ_ActiveFlg { get; set; }
        public string returnval { get; set; }
    }
}
