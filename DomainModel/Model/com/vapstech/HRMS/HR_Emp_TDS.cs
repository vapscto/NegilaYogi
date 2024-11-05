using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
  [Table("HR_Employee_TDS")]
  public class HR_Emp_TDS
    {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long HRETDS_Id { get; set; }
    public long MI_Id { get; set; }
    public long HRME_Id { get; set; }
    public long IMFY_Id { get; set; }
    public DateTime? HRETDS_DepositedDate { get; set; }
    public string HRETDS_BSRCode { get; set; }
    public string HRETDS_ChallanNo { get; set; }
    public bool HRETDS_ActiveFlg { get; set; }
    public long HRETDS_CreatedBy  { get; set; }
    public long HRETDS_UpdatedBy  { get; set; }
        public decimal? HRETDS_TaxDeposited { get; set; }

    }
}
