using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Emp_SalaryAdvance_Approval")]
    public class HR_Emp_SalaryAdvance_ApprovalDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRESAA_Id { get; set; }
        public long HRESA_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public String HRESAA_Remarks { get; set; }
        public DateTime HRESAA_Date { get; set; }
        public long HRESAA_SanctionLevel { get; set; }
        public bool HRESAA_ActiveFlag { get; set; }
      
    }
}
