using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Restricting_Day_Staff_ClassSection")]
    public class TT_Restricting_Day_Staff_ClassSectionDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       
        public long TTRDSCC_Id { get; set; }
        public long TTRDS_Id { get; set; }      
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long ISMS_Id { get; set; }   
        public long TTRDSCC_Periods { get; set; }
        public bool TTRDSCC_ActiveFlag { get; set; }
    }
}
