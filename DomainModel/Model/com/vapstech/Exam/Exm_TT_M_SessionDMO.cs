using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Exam
{
    [Table("Exm_TT_M_Session", Schema = "Exm")]
    public class Exm_TT_M_SessionDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ETTS_Id { get; set; }
        public long MI_Id { get; set; }
        public string ETTS_SessionName { get; set; }
        public string ETTS_StartTime { get; set; }
        public string ETTS_EndTime { get; set; }
        public string ETTS_Abreviation { get; set; }
        public bool ETTS_Activeflag { get; set; }
    }
}
