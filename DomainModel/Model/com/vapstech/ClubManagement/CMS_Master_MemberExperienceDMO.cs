using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.ClubManagement
{
    [Table("CMS_Master_Member_Experience", Schema = "CMS")]
    public class CMS_Master_MemberExperienceDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CMSMMEMEXP_Id { get; set; }
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
        public DateTime? CMSMMEMEXP_CreatedDate { get; set; }
        public long CMSMMEMEXP_CreatedBy { get; set; }
        public DateTime? CMSMMEMEXP_UpdatedDate { get; set; }
        public long CMSMMEMEXP_UpdatedBy { get; set; }

    }
}
