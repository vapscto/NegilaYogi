using DomainModel.Model.com.vapstech.COE;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.COE
{
    [Table("COE_Events_CourseBranch")]
    public class COE_Events_CourseBranchDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long COEECB_Id { get; set; }
        public int COEE_Id { get; set; }
        [ForeignKey("COEE_Id")]
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public bool COEECB_ActiceFlg { get; set; }
        public COE_EventsDMO COE_EventsDMO { get; set; }

    }
}
