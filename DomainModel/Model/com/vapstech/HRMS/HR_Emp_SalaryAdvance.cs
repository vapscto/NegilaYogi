using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
  [Table("HR_Emp_SalaryAdvance")]
  public class HR_Emp_SalaryAdvance:CommonParamDMO
    {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long HRESA_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime HRESA_EntryDate { get; set; }
        public int HRESA_AdvYear { get; set; }
        public string HRESA_AdvMonth { get; set; }
        public string HRESA_Remarks { get; set; }
        public string HRESA_AdvStatus { get; set; }
        public bool HRESA_ActiveFlag { get; set; }
        public decimal? HRESA_AppliedAmount { get; set; }
        public decimal? HRESA_SanctinedAmount { get; set; }
        public string HRESA_ModeOfPayment { get; set; }
        public long HRESA_ReferenceNo { get; set; }
        [ForeignKey("HRME_Id")]
        public virtual MasterEmployee MasterEmployee { get; set; }

        }
}
