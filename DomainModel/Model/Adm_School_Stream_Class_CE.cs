using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Adm_School_Stream_Class_CE")]
    public class Adm_School_Stream_Class_CE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ASSTCLCE_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMST_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMCE_Id { get; set; } 
        public bool ASSTCLCE_ActiveFlag { get; set; }
        public bool ASSTCLCE_CompulsoryFlg { get; set; }
    }
}