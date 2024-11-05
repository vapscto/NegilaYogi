using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.admission
{
    [Table("IVRM_Master_GovernmentBonds")]
    public class GovernmentBondDMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long IMGB_Id { get; set; }
        public string IMGB_Name { get; set; }
        public string IMGB_Description { get; set; }
        public long MI_Id { get; set; }
    }
}
