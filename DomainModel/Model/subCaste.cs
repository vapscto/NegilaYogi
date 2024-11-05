using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_Master_Subcaste")]
    public class subCaste 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IMSC_ID { get; set; }
        public long IMC_ID { get; set; }
        public string IMSC_Caste_Name { get; set; }
        public string IMSC_Caste_Desc { get; set; }
      
    }
}
