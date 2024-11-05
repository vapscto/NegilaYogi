using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Configuration")]
    public class HR_Configuration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRC_Id { get; set; }
        public long MI_Id { get; set; }
        public decimal? HRC_PFMaxAmt { get; set; }
        public decimal? HRC_FPFPer { get; set; }
        public decimal? HRC_EPFPer { get; set; }
        public bool HRC_AsPerEmpFlag { get; set; }
        public string HRC_PFAccNoPrefix { get; set; }
        public decimal? HRC_AccNo2 { get; set; }
        public decimal? HRC_AccNo21 { get; set; }
        public decimal? HRC_AccNo22 { get; set; }
        public int HRC_RetirementYrs { get; set; }
        public string HRC_ECodePrefix { get; set; }
        public decimal? HRC_ESIMax { get; set; }
        public decimal? HRC_ESIEmplrCont { get; set; }
        public string HRC_PayMethodFlg { get; set; }
        public bool? HRC_ArrSalaryFlag { get; set; }
        public bool? HRC_CummArrFlag { get; set; }
        public int HRC_SalaryFromDay { get; set; }
        public int HRC_SalaryToDay { get; set; }

        public decimal? HRC_ARTFPFPer { get; set; }
        public decimal? HRC_ARTEPFPer { get; set; }

        public decimal? HRC_ESIMaxAmount { get; set; }
        public decimal? HRC_AC2MinAmount { get; set; }
        public decimal? HRC_AC21MinAmount { get; set; }
        public decimal? HRC_AC22MinAmount { get; set; }
        public bool? HRC_SalAdvApprovalFlg { get; set; }
        public bool? HRC_LoanApprovalFlg { get; set; }
        public bool? HRC_LeaveApprovalFlg { get; set; }
        public bool? HRC_IncrementApprovalFlg { get; set; }
        public bool? HRC_ProbationApprovalFlg { get; set; }

        public bool? HRC_SalApprovalFlg { get; set; }

        public decimal? HRC_EducationCess { get; set; }
        public decimal? HRC_partAmaxflag { get; set; }
        public decimal? HRC_partBmaxflag { get; set; }

        public string HRC_IncrementOnFlag { get; set; }
        public string HRC_IncrementMonth { get; set; }
        public long? HRC_IncrementPercentage { get; set; }
        public long? HRC_SplIncrementOnceInYr { get; set; }

        public long? HRC_MinimumWorkingPeriod { get; set; }


        public bool? HRC_FixedIncrmentFlg { get; set; }

        public long? HRC_NoOfOptionalHolidays { get; set; }

        public long? HRC_IncrementOnceInMonths { get; set; }
       public int? HRC_AlertDay { get; set; }

    }
}
