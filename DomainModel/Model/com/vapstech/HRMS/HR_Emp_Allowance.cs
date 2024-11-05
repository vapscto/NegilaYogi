using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
  [Table("HR_Employee_Allowance")]
  public class HR_Emp_Allowance
    {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long HREAL_Id
        { get; set; }
    public long MI_Id { get; set; }
    public long HRME_Id { get; set; }
    public long IMFY_Id { get; set; }
    public long HRMAL_Id
        { get; set; }
    public decimal? HREAL_Allowance
        { get; set; }
   // public string HRETDS_ChallanNo { get; set; }
    public bool HREAL_ActiveFlg
        { get; set; }
    public long HREAL_CreatedBy
        { get; set; }
    public long HREAL_UpdatedBy
        { get; set; }

    }
}
