using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("PA_School_Application_CE")]
    public class PA_School_Application_CE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PASACE_Id { get; set; }
        public long PASR_Id { get; set; }
        public long ASMCE_Id { get; set; }
        public long PASACE_CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long PASACE_UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; } 
        public bool PASACE_ActiveFlg { get; set; }       
    }
}
