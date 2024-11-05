using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Master_Organization_EmailId")]
    public class OrganisationEmail : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MOE_Id { get; set; }
        public long MO_Id { get; set; }
        public string MOE_EmailId { get; set; }
        public string MOE_Flag { get; set; }

    }
}
