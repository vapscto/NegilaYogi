using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model
{
    [Table("Master_Organization_PhoneNo")]
    public class OrganisationPhone : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MOPN_Id { get; set; }
        public long MO_Id { get; set; }
        public long MOPN_PhoneNo { get; set; }
        public string MOP_Flag { get; set; }
    }
}
