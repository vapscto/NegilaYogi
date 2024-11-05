using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("MobileApplAuthentication")]
    public class MobileApplAuthenticationDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MAAN_Id { get; set; }
        public long MI_Id { get; set; }
        public string MAAN_AuthenticationKey { get; set; }
        
    }
}
