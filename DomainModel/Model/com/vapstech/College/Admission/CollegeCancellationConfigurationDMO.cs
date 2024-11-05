using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_AC_Config", Schema = "CLG")]
    public class CollegeCancellationConfigurationDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACACC_Id { get; set; }
        public long MI_Id { get; set; }        
        public int ACACC_DOAFlg { get; set; }
        public int ACACC_FromDays { get; set; }
        public int ACACC_ToDays { get; set; }
        public decimal ACACC_RefundAmountPer { get; set; }
        public decimal ACACC_CancellationPer { get; set; }
        public bool ACACC_ActiveFlag { get; set; }
        public long ACACC_CreatedBy { get; set; }
        public long ACACC_UpdatedBy { get; set; }
    }
}
