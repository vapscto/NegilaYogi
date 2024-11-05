using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Master_Institution_EmailId")]
    public class Institution_EmailId : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MIE_Id { get; set; }
        public long MI_Id { get; set; }
        public string MIE_EmailId { get; set; }
    }
}
