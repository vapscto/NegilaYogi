using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Adm_Master_Student_CE")]
    public class Adm_Master_Student_CE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AMSTCE_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMCE_Id { get; set; }
        public long AMSTCE_CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } 
        public long AMSTCE_UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; } 
        public bool AMSTCE_ActiveFlg { get; set; }       
    }
}
