using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Restricting_Period_Staff_ClassSection")]
    public class TT_Restricting_Period_Staff_ClassSectionDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public long TTRPSCC_Id { get; set; }
        public long TTRPS_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long TTRPSCC_Days { get; set; }
        public bool TTRPSCC_ActiveFlag { get; set; }
    }
}
