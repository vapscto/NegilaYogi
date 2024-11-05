using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.admission
{
    [Table("Adm_Master_Activity")]
    public class MasterActivityDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]     

        public long AMA_Id { get; set; }
        public string AMA_Activity { get; set; }
        public string AMA_Activity_Desc { get; set; }
        public long MI_Id { get; set; }

    }
}
