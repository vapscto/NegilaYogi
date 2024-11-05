using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HM_T_PolicyDetails")]
    public  class HM_T_PolicyDetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HMTPD_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HMTPD_MemberId { get; set; }
        public DateTime? HMTPD_PlanStartDate { get; set; }
        public DateTime? HMTPD_PlanEndDate { get; set; }
        public string HMTPD_PolicyName { get; set; }
        public string HMTPD_PlanName { get; set; }
        public string HMTPD_PolicyProvider { get; set; }
        public bool HMTPD_ActiveFlag { get; set; }
        public DateTime? HMTPD_CreatedDate { get; set; }
        public DateTime? HMTPD_UpdatedDate { get; set; }

    }
}
