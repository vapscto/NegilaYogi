using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("IVRM_AuditTrail_Deatils")]
    public class IVRM_AuditTrail_DeatilsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IATD_Id { get; set; }
        public long ITAT_Id { get; set; }
        public string IATD_ColumnName { get; set; }
        public string IATD_PreviousValue { get; set; }
        public string IATD_CurrentValue { get; set; }
       
    }
}
