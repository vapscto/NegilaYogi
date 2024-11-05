using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Master_Periodicity", Schema = "LIB")]
    public class MasterPeriodicityDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LMPE_Id { get; set; }
        public long MI_Id { get; set; }
        public string LMPE_PeriodicityName { get; set; }
        public bool LMPE_ActiveFlg { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }

    }
}
