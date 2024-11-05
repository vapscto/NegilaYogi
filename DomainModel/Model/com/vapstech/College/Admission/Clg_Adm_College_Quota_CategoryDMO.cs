using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_Quota_Category", Schema = "CLG")]
    public class Clg_Adm_College_Quota_CategoryDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACQC_Id { get; set; }
        public long MI_Id { get; set; }
        public string ACQC_CategoryName { get; set; }
        public string ACQC_CategoryCode { get; set; }
        public string ACQC_CategoryInfo { get; set; }
        public bool ACQC_ActiveFlg { get; set; }
    }
}
