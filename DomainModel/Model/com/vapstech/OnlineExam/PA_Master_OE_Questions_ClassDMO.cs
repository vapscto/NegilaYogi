using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.OnlineExam
{
    [Table("PA_Master_OE_Questions_Class")]
    public class PA_Master_OE_Questions_ClassDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PAMOEQC_Id { get; set; }
        public long PAMOEQ_Id { get; set; }
        public long ASMCL_Id { get; set; }
          
    }
}
