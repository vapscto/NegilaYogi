using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Restricting_Day_Subject")]
    public class TT_Restricting_Day_SubjectDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      
        public long TTRDSU_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long TTMD_Id { get; set; }
        public string TTRDSU_AllotedFlag { get; set; }
        public bool TTRDSU_ActiveFlag { get; set; }
        public bool TTRDSU_SUbSelcFlag { get; set; }
        public List<CLGTT_Restricting_Day_SubjectDMO> CLGTT_Restricting_Day_SubjectDMO { get; set; }
        

    }
}
