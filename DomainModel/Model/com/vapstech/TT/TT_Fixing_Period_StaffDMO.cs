using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Fixing_Period_Staff")]
    public class TT_Fixing_Period_StaffDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]       
        public long TTFPS_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long HRME_Id { get; set; }
        public int TTMP_Id { get; set; }
        public string TTFPS_AllotedFlag { get; set; }
        public bool TTFPS_ActiveFlag { get; set; }
        public bool TTFPS_SUbSelcFlag { get; set; }
        public List<CLGTT_Fixing_Period_StaffDMO> CLGTT_Fixing_Period_StaffDMO { get; set; }
    }
}
