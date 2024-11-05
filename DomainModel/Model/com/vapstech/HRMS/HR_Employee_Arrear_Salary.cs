using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Employee_Arrear_Salary")]
    public class HR_Employee_Arrear_Salary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HREAS_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HREAS_Year { get; set; }
        public string HREAS_Month { get; set; }
        public long HRMED_Id { get; set; }
        public decimal? HREAS_Amount { get; set; }
        public bool? HREAS_PaymentFlag { get; set; }


    }
}
