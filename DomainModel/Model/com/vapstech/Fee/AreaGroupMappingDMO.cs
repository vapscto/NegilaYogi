using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Group_Area_Mapping")]
    public class AreaGroupMappingDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FGAM_Id { get; set; }
        public long MI_Id { get; set; }
        public long FMG_Id { get; set; }
        public long TRMA_Id { get; set; }
        public bool FGAM_ActiveFlag { get; set; }
        public string FGAM_WayFlag { get; set; }
    }
}
