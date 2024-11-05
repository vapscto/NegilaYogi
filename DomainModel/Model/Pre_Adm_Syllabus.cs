using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Pre_Adm_Syllabus")]
    public class Pre_Adm_Syllabus : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PASL_ID { get; set; }
        public string PASL_Name { get; set; }

        public long MI_ID { get; set; }
    }
}
