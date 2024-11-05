using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  DomainModel.Model.com.vaps.admission
{
    [Table("Adm_Readmit_Student")]
    public class readmitstudentDMO : CommonParamDMO
    {
 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ARS_Id { get; set; }
        public long AMST_Id_Old { get; set; }
        public long ASMCL_Id_Old { get; set; }
        public long ASMAY_Id_OLd { get; set; }
        public long ASMCL_Id_New { get; set; }
        public long? userid { get; set; }
        public long ASMAY_Id_New { get; set; }
        public long AMST_Id_New { get; set; }
        public long MI_Id { get; set; }

    }
}
