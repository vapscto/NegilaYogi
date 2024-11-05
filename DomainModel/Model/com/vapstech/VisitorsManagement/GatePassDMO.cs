using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VisitorsManagement
{
    [Table("Adm_Gate_Pass_History")]
    public class GatePassDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AGPH_Id { get; set; }
        public long MI_Id { get; set; }
        public string AGPH_PassType { get; set; }
        public long AGPH_Idno { get; set; }
        public string AGPH_Dategiven { get; set; }
        public string AGPH_Remark { get; set; }

    }
}
