using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("Adm_Student_Transport_Application_Approve")]
    public class TransportApprovedDMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ASTAA_Id { get; set; }
        public long ASTA_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public DateTime? ASTAA_Date { get; set; }
    }
}
