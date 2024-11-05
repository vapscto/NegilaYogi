using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_Prv_Sch_Combination", Schema = "CLG")]
    public  class Adm_Prv_Sch_CombinationDMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ADMCB_ID { get; set; }
        public long MI_Id { get; set; }
        public string  ADMCB_NAME { get; set; }
        public bool ADMCB_Activeflag { get; set; }


    }
}
