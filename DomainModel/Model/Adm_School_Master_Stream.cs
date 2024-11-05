using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Adm_School_Master_Stream")]
    public class Adm_School_Master_Stream : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ASMST_Id { get; set; }
        public long MI_Id { get; set; }
        public string ASMST_StreamName { get; set; }
        public string ASMST_StreamCode { get; set; }
        public int ASMST_Order { get; set; }
        public bool ASMST_ActiveFlag { get; set; }       
    }
}
