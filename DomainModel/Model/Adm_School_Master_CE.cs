using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Adm_School_Master_CE")]
    public class Adm_School_Master_CE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ASMCE_Id { get; set; }
        public long MI_Id { get; set; }
        public string ASMCE_CEName { get; set; }
        public string ASMCE_CECode { get; set; }
        public int ASMCE_Order { get; set; }
        public bool ASMCE_ActiveFlag { get; set; }       
    }
}
