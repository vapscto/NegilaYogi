using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Master_IncomeTax")]
    public class HR_Master_IncomeTaxDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMIT_Id { get; set; }
        public long MI_Id { get; set; }
        public long IMFY_Id { get; set; }
        public long HRMIT_GenderFlag { get; set; }
        public string HRMIT_AgeFlag { get; set; }
        public Int32? HRMIT_FromAge { get; set; }
        public Int32? HRMIT_ToAge { get; set; }
        public bool HRMIT_ActiveFlag { get; set; }

        [ForeignKey("HRMIT_GenderFlag")]
        public virtual IVRM_Master_Gender gendername { get; set; }

        [ForeignKey("IMFY_Id")]
        public virtual IVRM_Master_FinancialYear financilYear { get; set; }
    }
}
