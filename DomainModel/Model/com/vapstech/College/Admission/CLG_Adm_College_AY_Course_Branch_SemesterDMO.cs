using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_AY_Course_Branch_Semester", Schema = "CLG")]
    public class CLG_Adm_College_AY_Course_Branch_SemesterDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACAYCBS_Id { get; set; }
        public long MI_Id { get; set; }
        public long ACAYCB_Id { get; set; }
        public long AMSE_Id { get; set; }

        
        public DateTime ACAYCBS_SemStartDate { get; set; }
        public DateTime ACAYCBS_SemEndDate { get; set; }
        public int ACAYCBS_SemOrder { get; set; }
        public bool ACAYCBS_ActiveFlag { get; set; }


    }
}
