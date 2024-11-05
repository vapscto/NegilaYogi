using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Student_Route_FeeGroup", Schema = "TRN")]
    public class TR_Student_Route_FeeGroupDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRSRFG_Id { get; set; }
        public long TRSR_Id { get; set; }
        public long FMG_Id { get; set; }
        public bool TRSRFG_ActiveFlg { get; set; }

    }
}
