using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Fixing_Period_Subject")]
    public class TT_Fixing_Period_SubjectDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]     

        public long TTFPSU_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ISMS_Id { get; set; }
        public int TTMP_Id { get; set; }
        public string TTFPSU_AllotedFlag { get; set; }
        public bool TTFPSU_ActiveFlag { get; set; }
        public bool TTFPSU_SUbSelcFlag { get; set; }
        public List<CLGTT_Fixing_Period_SubjectDMO> CLGTT_Fixing_Period_SubjectDMO { get; set; }
        
    }
}
