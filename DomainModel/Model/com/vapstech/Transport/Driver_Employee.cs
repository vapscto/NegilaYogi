using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Driver_Employee", Schema = "TRN")]
    public class Driver_Employee : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRDE_Id { get; set; }
        public long TRMD_Id { get; set; }
        public long HRME_Id { get; set; }
    }
}
