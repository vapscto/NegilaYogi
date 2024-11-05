using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model
{
    [Table("Master_Organization_MobileNo")]
    public class OrganisationMobile : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MOMN_Id { get; set; }
        public long MO_Id { get; set; }
        public long MOMN_MobileNo { get; set; }

        public string MOM_Flag { get; set; }
    }
}
