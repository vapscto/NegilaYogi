using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.ClubManagement
{
  public   class CMS_Master_Member_ExperienceDTO
    {
        public long CMSMMEMEXP_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long CMSMMEM_Id { get; set; }
        public string CMSMMEMEXP_OrganisationName { get; set; }
        public string CMSMMEMEXP_OrganisationAddress { get; set; }
        public string CMSMMEMEXP_Department { get; set; }
        public string CMSMMEMEXP_Designation { get; set; }
        public DateTime? CMSMMEMEXP_JoinDate { get; set; }
        public DateTime? CMSMMEMEXP_ExitDate { get; set; }
        public long CMSMMEMEXP_NoOfYears { get; set; }
        public long CMSMMEMEXP_NoofMonths { get; set; }
        public decimal CMSMMEMEXP_AnnualSalary { get; set; }
        public string CMSMMEMEXP_ReasonForLeaving { get; set; }
        public bool CMSMMEMEXP_ActiveFlg { get; set; }
        public string returnval { get; set; }

    }
}
