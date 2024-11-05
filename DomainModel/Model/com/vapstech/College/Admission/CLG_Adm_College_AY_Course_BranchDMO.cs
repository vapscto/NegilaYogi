using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_AY_Course_Branch", Schema = "CLG")]
    public class CLG_Adm_College_AY_Course_BranchDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACAYCB_Id { get; set; }
        public long MI_Id { get; set; }
        public long ACAYC_Id { get; set; }
        public long AMB_Id { get; set; }
        public DateTime? ACAYCB_PreAdm_FDate { get; set; }
        public DateTime? ACAYCB_PreAdm_TDate { get; set; }
        public DateTime? ACAYB_ReferenceDate { get; set; }
        public bool ACAYCB_ActiveFlag { get; set; }
        public List<CLG_Adm_College_AY_Course_Branch_SemesterDMO> CLG_Adm_College_AY_Course_Branch_SemesterDMO { get; set; }



    }
}
