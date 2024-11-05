using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Master_IncomeTax_Cess")]
    public class HR_Master_IncomeTax_CessDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long HRMITC_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMITC_CessName { get; set; }
        public bool? HRMITC_ActiveFlag { get; set; }
    }
}
