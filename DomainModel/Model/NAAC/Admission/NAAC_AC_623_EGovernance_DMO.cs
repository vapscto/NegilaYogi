using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_623_EGovernance")]
    public class NAAC_AC_623_EGovernance_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long NCAC623EGOV_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC623EGOV_ImpYear { get; set; }
        public string NCAC623EGOV_GovernanceArea { get; set; }
        public string NCAC623EGOV_VendorName { get; set; }
        public string NCAC623EGOV_VendorAddress { get; set; }
        public long NCAC623EGOV_VendorPhoneNo { get; set; }
        public string NCAC623EGOV_VendorEmailId { get; set; }

        public bool NCAC623EGOV_ActiveFlg { get; set; }
        public long NCAC623EGOV_CreatedBy { get; set; }
        public long NCAC623EGOV_UpdatedBy { get; set; }
        public DateTime NCAC623EGOV_CreatedDate { get; set; }
        public DateTime NCAC623EGOV_UpdatedDate { get; set; }
        public string NCAC623EGOV_StatusFlg { get; set; }
        public bool? NCAC623EGOV_ApprovedFlg { get; set; }
        public string NCAC623EGOV_Remarks { get; set; }

        public List<NAAC_AC_623_EGovernance_Files_DMO> NAAC_AC_623_EGovernance_Files_DMO { get; set; }
    }
}
