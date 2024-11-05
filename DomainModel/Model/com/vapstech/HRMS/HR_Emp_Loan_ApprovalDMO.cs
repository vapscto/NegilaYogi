using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Emp_Loan_Approval")]
    public class HR_Emp_Loan_ApprovalDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRELA_Id { get; set; }
        public long HREL_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public string HRESAA_Remarks { get; set; }
        public DateTime HRELA_Date { get; set; }
        public string HRELA_SanctionLevel { get; set; }
        public bool HRELA_ActiveFlag { get; set; }
    }
}
