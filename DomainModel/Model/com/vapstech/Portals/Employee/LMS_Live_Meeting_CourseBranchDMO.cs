using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("LMS_Live_Meeting_CourseBranch")]
    public class LMS_Live_Meeting_CourseBranchDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LMSLMEETCOBR_Id { get; set; }
        public long  LMSLMEET_Id { get; set; }
        public long  ASMAY_Id { get; set; }
        public long  AMCO_Id { get; set; }
        public long  AMB_Id { get; set; }
        public long  AMSE_Id { get; set; }
        public long  ISMS_Id { get; set; }
        public long ACMS_Id { get; set; }
        public bool  LMSLMEETCOBR_ActiveFlg { get; set; }
        public DateTime  LMSLMEETCOBR_CreatedDate { get; set; }
        public DateTime LMSLMEETCOBR_UpdatedDate { get; set; }
        public long  LMSLMEETCOBR_CreatedBy { get; set; }
        public long  LMSLMEETCOBR_UpdatedBy { get; set; }

    }
}
