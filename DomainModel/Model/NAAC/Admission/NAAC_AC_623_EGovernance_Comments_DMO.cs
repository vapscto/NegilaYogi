using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_623_EGovernance_Comments")]
    public class NAAC_AC_623_EGovernance_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC623EGOVC_Id { get; set; }
        public string NCAC623EGOVC_Remarks { get; set; }
        public long? NCAC623EGOVC_RemarksBy { get; set; }
        public string NCAC623EGOVC_StatusFlg { get; set; }
        public bool? NCAC623EGOVC_ActiveFlag { get; set; }
        public long? NCAC623EGOVC_CreatedBy { get; set; }
        public DateTime? NCAC623EGOVC_CreatedDate { get; set; }
        public long? NCAC623EGOVC_UpdatedBy { get; set; }
        public DateTime? NCAC623EGOVC_UpdatedDate { get; set; }
        public long NCAC623EGOV_Id { get; set; }
    }
}
