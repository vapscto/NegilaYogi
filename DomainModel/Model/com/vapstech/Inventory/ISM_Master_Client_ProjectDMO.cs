using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("ISM_Master_Client_Project")]
    public class ISM_Master_Client_ProjectDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMMCLTPR_Id { get; set; }
        public long ISMMCLT_Id { get; set; }
        public long ISMMPR_Id { get; set; }
        public long ISMMMD_Id { get; set; }
        public bool ISMMCLTPR_ActiveFlag { get; set; }
        public long ISMMCLTPR_CreatedBy { get; set; }
        public long ISMMCLTPR_UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string ISMMCLTPR_ProposalRefNo { get; set; }
        public DateTime? ISMMCLTPR_ProposalSentDate { get; set; }
        public DateTime? ISMMCLTPR_DealClosureDate { get; set; }
        public string ISMMCLTPR_MOURefNo { get; set; }
        public DateTime? ISMMCLTPR_MOUDate { get; set; }
        public long? ISMMCLTPR_MOURepresentedBy { get; set; }
        public DateTime? ISMMCLTPR_MOUStartDate { get; set; }
        public DateTime? ISMMCLTPR_MOUEndDate { get; set; }
        public string ISMMCLTPR_NodalOfficerName { get; set; }
        public long? ISMMCLTPR_NodalOfficerContactNo { get; set; }
        public string ISMMCLTPR_NodalOfficerEmailId { get; set; }
        public string ISMMCLTPR_ProjectDuration { get; set; }
        public long? ISMMCLTPR_TotalStudent { get; set; }
        public decimal? ISMMCLTPR_CostPerStudent { get; set; }
        public decimal? ISMMCLTPR_EnhancementPerYr { get; set; }
       // public string ISMMCLTPR_WorkOrder { get; set; }
    }
}
