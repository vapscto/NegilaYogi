using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_AY_Course", Schema = "CLG")]
    public class CLG_Adm_College_AY_CourseDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACAYC_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public DateTime ACAYC_From_Date { get; set; }
        public DateTime ACAYC_To_Date { get; set; }
        public int ACAYC_NoOfSEM { get; set; }
        public bool ACAYC_ActiveFlag { get; set; }
        public List<CLG_Adm_College_AY_Course_BranchDMO> CLG_Adm_College_AY_Course_BranchDMO { get; set; }
    }
}
