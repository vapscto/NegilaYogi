using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("IVRM_Master_Marital_Status")]
    public class IVRM_Master_Marital_Status : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMMMS_Id { get; set; }
        public long MI_Id { get; set; }
        public string IVRMMMS_MaritalStatus { get; set; }
        public bool? IVRMMMS_ActiveFlag { get; set; }
    }
}
