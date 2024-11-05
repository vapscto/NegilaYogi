using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_Seat_Distribution", Schema = "CLG")]
    public class Clg_Adm_College_Seat_DistributionDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACSCD_Id { get; set; }
        public long MI_Id { get; set; }
        public long ACSC_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }

        public long AMSE_Id { get; set; }
        public long ACQ_Id { get; set; }
        public long ACQC_Id { get; set; }
        public decimal ACSCD_SeatPer { get; set; }
        public long ACSCD_SeatNos { get; set; }
        public string ACSCD_Remarks { get; set; }
        public bool ACSCD_ActiveFlg { get; set; }
    }
}
