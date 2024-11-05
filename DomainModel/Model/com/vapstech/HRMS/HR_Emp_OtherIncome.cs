using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
  [Table("HR_Employee_OtherIncome")]
  public class HR_Emp_OtherIncome
    {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HREOI_Id

        { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long IMFY_Id { get; set; }
        public long HRMOI_Id

        { get; set; }
        public decimal? HREOI_OtherIncome

        { get; set; }
        // public string HRETDS_ChallanNo { get; set; }
        public bool HREOI_ActiveFlg

        { get; set; }
        public long HREOI_CreatedBy

        { get; set; }
        public long HREOI_UpdatedBy

        { get; set; }
    }
}
