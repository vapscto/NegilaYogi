using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Portals.IVRM
{
   [Table("IVRM_HOD_Branch", Schema = "CLG")]
   public class IVRM_HOD_Branch_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long IHODB_Id { get; set; }
        public long IHOD_Id { get; set; }
        public long AMB_Id { get; set; }
        public bool IHODB_ActiveFlag { get; set; }
    }
}
