using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Master_ProfessionalTax")]
    public class HR_Master_ProfessionalTaxDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMPT_Id { get; set; }
        public long MI_Id { get; set; }
        public decimal? HRMPT_SalaryFrom { get; set; }
        public decimal? HRMPT_SalaryTo { get; set; }
        public decimal? HRMPT_PTax { get; set; }
        public bool HRMPT_ActiveFlag { get; set; }
    }
}
