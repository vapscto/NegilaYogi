using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.College.Preadmission
{
    [Table("PA_College_Student_CBPreference", Schema = "CLG")]
    public class PA_College_Student_CBPreference : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long PACSTCBO_Id { get; set; }
        public long PACA_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public int PACSTCBO_Order { get; set; }
        public bool PACSTCBO_ActiveFlg { get; set; }

    }
}
