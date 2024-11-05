using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_Course_Branch_Semester_Mapping", Schema ="CLG")]
    public class AdmCourseBranchSemesterMappingDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AMCOBMS_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCOBM_Id { get; set; }
        public long AMSE_Id { get; set; }
        public bool AMCOBMS_ActiveFlg { get; set; }
    }
}
