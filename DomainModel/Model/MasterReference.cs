using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Preadmission_Master_Reference")]
    public class MasterReference : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PAMR_Id { get; set; }

        public string PAMR_ReferenceName{ get; set; }
        public string PAMR_ReferenceDesc { get; set; }

    }
}
