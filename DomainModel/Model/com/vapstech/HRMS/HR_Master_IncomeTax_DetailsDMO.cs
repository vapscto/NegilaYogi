using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Master_IncomeTax_Details")]
    public class HR_Master_IncomeTax_DetailsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMITD_Id { get; set; }
        public long HRMIT_Id { get; set; }
        public decimal? HRMITD_AmountFrom { get; set; }
        public decimal? HRMITD_AmountTo { get; set; }
        public decimal? HRMITD_IncomeTax { get; set; }
        public bool HRMITD_ActiveFlag { get; set; }

        [ForeignKey("HRMIT_Id")]
        public virtual HR_Master_IncomeTaxDMO MasterIncomeTax { get; set; }

       
    }
}
