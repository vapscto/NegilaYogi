using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Fixing_Day_Staff_ClassSection")]
    public class TT_Fixing_Day_Staff_ClassSectionDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       
        public long TTFDSCC_Id { get; set; }
        public long TTFDS_Id { get; set; }      
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long ISMS_Id { get; set; }   
        public long TTFDSCC_Periods { get; set; }
        public bool TTFDSCC_ActiveFlag { get; set; }
    }
}
