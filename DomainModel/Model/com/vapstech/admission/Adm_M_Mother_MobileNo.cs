using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.admission
{
    [Table("Adm_Master_MotherMobileNo")]
    public class Adm_M_Mother_MobileNo:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AMSTMMNO_Id { get; set; }
        public long MI_Id { get; set; }
        [ForeignKey("AMST_Id")]
        public long AMST_Id { get; set; }
        public long? AMST_MotherMobileNo { get; set; }
        public long? AMSTMMNO_CreatedBy { get; set; }
        public long? AMSTMMNO_UpdatedBy { get; set; }

        public string AMSTMMNO_CoutryCode { get; set; }
    }
}
