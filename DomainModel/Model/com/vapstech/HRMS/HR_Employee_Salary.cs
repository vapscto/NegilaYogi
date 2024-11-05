using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Employee_Salary")]
    public class HR_Employee_Salary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRES_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HRES_Year { get; set; }
        public string HRES_Month { get; set; }
        public Double? HRES_WorkingDays { get; set; }
        public string HRES_DailyRates { get; set; }
        public decimal? HRES_EPF { get; set; }
        public decimal? HRES_FPF { get; set; }
        public decimal? HRES_Ac21 { get; set; }
        public decimal? HRES_Ac22 { get; set; }
        public decimal? HRES_Ac5 { get; set; }
        public DateTime? HRES_FromDate { get; set; }
        public DateTime? HRES_ToDate { get; set; }
        public bool? HRES_ArrearRegFlag { get; set; }
        public string HRES_BankCashFlag { get; set; }
        public long? HRMGT_Id { get; set; }
        public long? HRMD_Id { get; set; }
        public long? HRMDES_Id { get; set; }
        public string HRES_BankCode { get; set; }
        public string HRES_AccountNo { get; set; }

        public decimal? HRES_ESIEmplr { get; set; }
        public bool? HRES_ApproveFlg { get; set; }

    }
}
