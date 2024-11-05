using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Portals.HOD
{
    [Table("IVRM_HOD_Staff")]
    public class IVRM_HOD_Staff_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long IHODS_Id { get; set; }

        public long IHOD_Id { get; set; }
        public long HRME_Id { get; set; }
        public bool IHODS_ActiveFlag { get; set; }
    }
}
