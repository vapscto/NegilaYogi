using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Master_Area", Schema = "TRN")]
    public class MasterAreaDMO :CommonParamDMO
    {     

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRMA_Id { get; set; }
        public long MI_Id { get; set; }
        public string TRMA_AreaName { get; set; }
        public string TRMA_AliasName { get; set; }
        public bool TRMA_ActiveFlg { get; set; }

    }
}
