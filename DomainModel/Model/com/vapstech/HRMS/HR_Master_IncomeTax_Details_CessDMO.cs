using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Master_IncomeTax_Details_Cess")]
    public class HR_Master_IncomeTax_Details_CessDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMITDC_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMITD_Id { get; set; }
        public long HRMITC_Id { get; set; }
        public decimal HRMITDC_Amount { get; set; }
        public bool HRMITDC_ActiveFlag { get; set; }
    }
}
