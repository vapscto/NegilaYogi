using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("Adm_Student_Trans_Appl_App_Coll")]
    public class CLGTransportApprovedDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ASTAACO_Id { get; set; }
        public long ASTACO_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public DateTime? ASTAACO_Date { get; set; }
    }
}
