using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Employee_Salary_Details")]
    public class HR_Employee_Salary_Details
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long HRESD_Id { get; set; }
        public long HRES_Id { get; set; }
        public long HRMED_Id { get; set; }
        public decimal? HRESD_Amount { get; set; }
    }
}
