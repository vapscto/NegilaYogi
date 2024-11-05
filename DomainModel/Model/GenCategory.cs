using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Adm_m_Fees_Consession_Master")]
    public class GenCategory : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long Consession_type_Id { get; set; }
        public string Consession_type_name { get; set; }

        public long MI_Id { get; set; }
    }
}
