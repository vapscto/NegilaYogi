using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
  [Table("HR_Master_EarningsDeductions_Type")]
  public class HR_Master_EarningsDeductions_Type:CommonParamDMO
    {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long HRMEDT_Id { get; set; }
    public string HRMEDT_EarnDedType { get; set; }
    public bool HRMEDT_ActiveFlag{ get; set; }

}
}
