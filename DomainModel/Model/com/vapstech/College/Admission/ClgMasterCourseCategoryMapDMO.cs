using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_Course_Category_Mapping", Schema = "CLG")]
    public class ClgMasterCourseCategoryMapDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AMCOCM_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMCOC_Id { get; set; }
        public bool AMCOCM_ActiveFlg { get; set; }

    }
}
