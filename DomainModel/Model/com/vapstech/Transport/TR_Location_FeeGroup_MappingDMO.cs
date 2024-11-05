using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Location_FeeGroup_Mapping", Schema = "TRN")]
    public class TR_Location_FeeGroup_MappingDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRLFM_Id { get; set; }       
        public long TRML_Id { get; set; }
        public long FMG_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long MI_Id { get; set; }

        public bool TRLFM_ActiveFlag { get; set; }
        public string TRLFM_WayFlag { get; set; }
    }
}
