using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Adm_School_Stream_Class")]
    public class Adm_School_Stream_Class : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ASSTCL_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMST_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public bool ASSTCL_ActiveFlag { get; set; }       
    }
}
