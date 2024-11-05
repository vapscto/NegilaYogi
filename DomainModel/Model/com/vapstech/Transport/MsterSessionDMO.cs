using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Master_Session", Schema = "TRN")]
    public class MsterSessionDMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRMS_Id { get; set; }
        [ForeignKey("MI_Id")]
        public long MI_Id { get; set; }
        public string TRMS_SessionName { get; set; }
        public string TRMS_SessionDesc { get; set; }
        public string TRMS_Flag { get; set; }
        public bool TRMS_ActiveFlg { get; set; }

    }
}
