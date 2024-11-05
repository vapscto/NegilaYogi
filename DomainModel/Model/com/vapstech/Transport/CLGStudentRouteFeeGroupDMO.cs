using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Student_Route_FeeGroup_College", Schema = "TRN")]

    public class CLGStudentRouteFeeGroupDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
   
        public long TRSRFGCO_Id { get; set; }
        public long TRSRCO_Id { get; set; }
        public long FMG_Id { get; set; }
        public bool TRSRFGCO_ActiveFlg { get; set; }
       
    }
}
