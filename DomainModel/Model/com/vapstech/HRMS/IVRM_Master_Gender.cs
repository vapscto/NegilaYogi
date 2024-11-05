using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("IVRM_Master_Gender")]
    public class IVRM_Master_Gender:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMMG_Id { get; set; }
        public long MI_Id { get; set; }
        public string IVRMMG_GenderName { get; set; }
        public bool IVRMMG_ActiveFlag { get; set; }
    }
}
