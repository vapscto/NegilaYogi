using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Master_Institution_MobileNo")]
    public class Institution_MobileNo : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MIMN_Id { get; set; }
        public long MI_Id { get; set; }
        public long MIMN_MobileNo { get; set; }
    }
}
