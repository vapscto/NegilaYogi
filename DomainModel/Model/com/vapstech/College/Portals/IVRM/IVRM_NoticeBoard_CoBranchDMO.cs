using DomainModel.Model.com.vapstech.Portals.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Portals.IVRM
{
    [Table("IVRM_NoticeBoard_CoBranch", Schema = "CLG")]
    public class IVRM_NoticeBoard_CoBranchDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INTBCB_Id { get; set; }
        public long INTB_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public bool INTBCB_ActiveFlag { get; set; }

        public IVRM_NoticeBoardDMO IVRM_NoticeBoardDMO { get; set; }
    }
}
