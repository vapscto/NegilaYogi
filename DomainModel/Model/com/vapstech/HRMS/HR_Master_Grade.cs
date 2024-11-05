using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
  [Table("HR_Master_Grade")]
  public class HR_Master_Grade:CommonParamDMO
    {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long HRMG_Id { get; set; }
    public long MI_Id { get; set; }
    public string HRMG_GradeName { get; set; }
    public string HRMG_GradeDisplayName { get; set; }
    public string HRMG_PayScaleRange { get; set; }
    public decimal HRMG_PayScaleFrom { get; set; }
    public decimal HRMG_IncrementOf { get; set; }
    public decimal HRMG_PayScaleTo { get; set; }
    public Int32? HRMG_Order { get; set; }
    public bool HRMG_ActiveFlag { get; set; }
  }
}
