using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.admission
{
    [Table("Adm_Master_Student_EmailId")]
    public class Adm_M_Student_Email_Id :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AMSTE_Id { get; set; }
        [ForeignKey("AMST_Id")]
        public long AMST_Id { get; set; }
        public string AMSTE_EmailId { get; set; }
        public long? AMSTE_CreatedBy { get; set; }
        public long? AMSTE_UpdatedBy { get; set; }
    }
}
