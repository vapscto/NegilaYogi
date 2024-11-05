using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Restricting_Day_Subject_ClassSection")]
    public class TT_Restricting_Day_Subject_ClassSectionDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       
        public long TTRDSUCC_Id { get; set; }
        public long TTRDSU_Id { get; set; }      
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long HRME_Id { get; set; }
        public long TTRDSUCC_Periods { get; set; }
        
        public bool TTRDSUCC_ActiveFlag { get; set; }
    }
}
