using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.mobile
{
    public class EmployeeSalaryDTO
    {
        public class input
        {
            public long HRME_ID { get; set; }
            public long MI_Id { get; set; }

            public string HRES_YEAR { get; set; }

            public string HRES_MONTH { get; set; }
         
        }

        public class Output
        {
            public long HRME_ID { get; set; }
            public string HRME_EmployeeFirstName { get; set; }
            public string HRME_EmployeeMiddleName { get; set; }
            public string HRME_EmployeeLastName { get; set; }
            public string HRME_EmployeeCode { get; set; }
            public DateTime? HRME_DOB { get; set; }
            public string HRMEM_EmailId { get; set; }
            public long Emp_MobileNo { get; set; }

            public DateTime? HRME_DOJ { get; set; }
            // public string HRME_Photo { get; set; }

            // public Array empD { get; set; }

            public string Hrme_designation { get; set; }

            public string hrme_deptname { get; set; }
            public string HRME_PFAccNo { get; set; }
            public Double? HRES_WorkingDays { get; set; }
            public string logo { get; set; }


            public Array salarylist { get; set; }
            public Array EarningList {get;set;}
            public Array DeductionList { get; set; }
         

          
           // public Array TotalDeduction { get; set; }
            //public Array TotalSumEarning { get; set; }
            //public Array TotalSumDeduction { get; set; }
        }
        public Array EmployeePortal_SalaryD { get; set; }
     
    public Array salarylist { get; set; }

        public Array TotalEarning { get; set; }

        public Array totalDeduction { get; set; }
        //public Array TotalSumEarning { get; set; }
        //public Array TotalSumDeduction { get; set; }
        public class EOutput
        {
            public string earningname { get; set; }
            public decimal? Amount { get; set; }
        }

        public class DOutput
        {
            public string Deductionname { get; set; }
            public decimal? Amount { get; set; }
        }

        //public class AddEarning
        //{
        //    public decimal? E1 { get; set; }

        //}

        //public class AddDeduction
        //{
        //    public decimal? D1 { get; set; }

        //}
    }
}
