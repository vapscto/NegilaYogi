using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
  [Table("HR_Master_Loan")]
  public class HRMasterLoan:CommonParamDMO
    {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long HRMLN_Id { get; set; }
    public long MI_Id { get; set; }
    public string HRML_LoanType { get; set; }
    public decimal? HRML_Max { get; set; }
    public bool HRMLN_ActiveFlag { get; set; }

  }
}
