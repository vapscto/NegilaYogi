using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_Atten_Batch_Subject_Students", Schema ="CLG")]
    public class Adm_College_Atten_Batch_Subject_StudentsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACABSS_Id { get; set; }    
        [ForeignKey("ACABS_Id")]
        public long ACABS_Id { get; set; }
        public long AMCST_Id { get; set; }
        public bool ACABSS_ActiveFlg { get; set; }        
    }
}
