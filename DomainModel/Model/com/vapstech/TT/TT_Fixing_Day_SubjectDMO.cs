using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Fixing_Day_Subject")]
    public class TT_Fixing_Day_SubjectDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      
        public long TTFDSU_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long TTMD_Id { get; set; }
        public string TTFDSU_AllotedFlag { get; set; }
        public bool TTFDSU_ActiveFlag { get; set; }
        public bool TTFDSU_SUbSelcFlag { get; set; }
        public List<CLGTT_Fixing_Day_SubjectDMO> CLGTT_Fixing_Day_SubjectDMO { get; set; }
        

    }
}
