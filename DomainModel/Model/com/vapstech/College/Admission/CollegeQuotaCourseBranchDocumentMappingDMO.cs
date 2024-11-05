using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_Quota_Course_Documents", Schema = "CLG")]
    public class CollegeQuotaCourseBranchDocumentMappingDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACQCD_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long ACQ_Id { get; set; }
        public long AMSMD_Id { get; set; }
        public bool ACQCD_CompulsoryFlg { get; set; }
        public bool ACQCD_ActiveFlg { get; set; }
    }
}
