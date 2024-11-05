using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.admission
{
    [Table("Adm_Master_FatherEmail_Id")]
    public class Adm_Master_Father_Email :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AMSTFEMAIL_Id { get; set; }
        public long MI_Id { get; set; }
        [ForeignKey("AMST_Id")]
        public long AMST_Id { get; set; }
        public string AMST_FatheremailId { get; set; }
        public long? AMSTFEMAIL_CreatedBy { get; set; }
        public long? AMSTFEMAIL_UpdatedBy { get; set; }
    }
}
