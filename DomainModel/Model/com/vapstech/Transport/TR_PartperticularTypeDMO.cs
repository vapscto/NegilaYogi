using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_PartperticularType", Schema = "TRN")]
    public class TR_PartperticularTypeDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRPAPT_Id { get; set; }
        public long MI_Id { get; set; }
        public string TRPAPT_PType { get; set; }
        public bool TRPAPT_ActiveFlag { get; set; }
    }
}
