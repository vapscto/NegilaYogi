using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Fixing_Day_Staff")]
    public class TT_Fixing_Day_StaffDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public long TTFDS_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long HRME_Id { get; set; }
        public long TTMD_Id { get; set; }
        public string TTFDS_AllotedFlag { get; set; }
        public bool TTFDS_ActiveFlag { get; set; }
        public bool TTFDS_SUbSelcFlag { get; set; }
        public List<CLGTT_Fixing_Day_StaffDMO> CLGTT_Fixing_Day_StaffDMO { get; set; }

    }
}
