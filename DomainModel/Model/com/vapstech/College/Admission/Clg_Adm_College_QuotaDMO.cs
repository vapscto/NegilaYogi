using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_Quota", Schema = "CLG")]
    public class Clg_Adm_College_QuotaDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACQ_Id { get; set; }
        public long MI_Id { get; set; }
        public string ACQ_QuotaName { get; set; }
        public string ACQ_QuotaCode { get; set; }
        public string ACQ_QuotaInfo { get; set; }
        public bool ACQ_ActiveFlg { get; set; }
    }
}
