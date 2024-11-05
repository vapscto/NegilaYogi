using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Portals.HOD
{
    [Table("IVRM_HOD")]
    public class HOD_DMO: CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IHOD_Id { get; set; }

        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public bool IHOD_ActiveFlag { get; set; }
        public string IHOD_Flg { get; set; }
        
        public List<IVRM_HOD_Class_DMO> IVRM_HOD_Class_DMO { get; set; }
        public List<IVRM_HOD_Staff_DMO> IVRM_HOD_Staff_DMO { get; set; }
    }
}
