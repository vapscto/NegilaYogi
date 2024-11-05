using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_Course_Branch_Mapping", Schema = "CLG")]
    public class Adm_Course_Branch_MappingDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AMCOBM_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public bool AMCOBM_ActiveFlg { get; set; }
        public string AMCOBM_Code { get; set; }
        public bool? AMCOBM_CBCSFlg { get; set; }
        public bool? AMCOBM_ElectiveFlg { get; set; }
        public long? AMCOBM_CBCSIntroYear { get; set; }
        public string AMCOBM_FileName { get; set; }
        public string AMCOBM_FilePath { get; set; }
        public long? AMCOBM_ElectiveIntroYear { get; set; }
        public List<AdmCourseBranchSemesterMappingDMO> AdmCourseBranchSemesterMappingDMO { get; set; }
    }
}
