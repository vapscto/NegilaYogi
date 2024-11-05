using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_Transaction_Numbering_Type")]
    public class Transaction_Numbering_Type : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ITNT_Id { get; set; }
        public string ITNT_Type { get; set; }
    }
}
