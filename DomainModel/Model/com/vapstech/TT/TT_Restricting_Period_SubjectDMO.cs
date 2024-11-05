using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Restricting_Period_Subject")]
    public class TT_Restricting_Period_SubjectDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]     

        public long TTRPSU_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ISMS_Id { get; set; }
        public int TTMP_Id { get; set; }
        public string TTRPSU_AllotedFlag { get; set; }
        public bool TTRPSU_ActiveFlag { get; set; }
        public bool TTRPSU_SUbSelcFlag { get; set; }
        public List<CLGTT_Restricting_Period_SubjectDMO> CLGTT_Restricting_Period_SubjectDMO { get; set; }
        
    }
}
