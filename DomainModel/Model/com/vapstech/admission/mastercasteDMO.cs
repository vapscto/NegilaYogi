using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.admission
{
    [Table("IVRM_Master_Caste")]
    public class mastercasteDMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IMC_Id { get; set; }
        public long? IMCC_Id { get; set; }
        public string IMC_CasteName { get; set; }
        public string IMC_CasteDesc { get; set; }
        public long? MI_Id { get; set; }
    }
}
