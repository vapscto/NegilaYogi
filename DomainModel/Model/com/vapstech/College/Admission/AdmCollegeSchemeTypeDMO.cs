using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_SchemeType", Schema = "CLG")]
    public class AdmCollegeSchemeTypeDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACST_Id { get; set; }
        public long MI_Id { get; set; }
        public string ACST_SchmeType { get; set; }
        public bool ACST_ActiveFlg { get; set; }
       
    }
}
