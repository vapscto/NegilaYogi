using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_Attendance_Batch", Schema ="CLG")]
    public class Adm_College_Attendance_BatchDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACAB_Id { get; set; }    
        public long MI_Id { get; set; }
        public string ACAB_BatchName { get; set; }
        public int ACAB_StudentStrength { get; set; }        
    }
}
