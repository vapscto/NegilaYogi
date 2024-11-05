using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
  [Table("HR_Emp_Salary_Approval")]
  public class HR_Emp_Salary_Approval : CommonParamDMO
    {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long HRESA_Id { get; set; }
    public long HRES_Id { get; set; }
    public long IVRMUL_Id { get; set; }
    public string HRESA_Remarks { get; set; }
        public long HRESA_SanctionLevel { get; set; }
        public bool HRESA_ActiveFlag { get; set; }
        public DateTime? HRESA_Date { get; set; }
    }
}
