using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.admission
{
    [Table("IVRM_Master_Religion")]
    public class MasterReligionDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMMR_Id { get; set; }
        public string IVRMMR_Name { get; set; }
        public bool Is_Active { get; set; }
      
    }
}
