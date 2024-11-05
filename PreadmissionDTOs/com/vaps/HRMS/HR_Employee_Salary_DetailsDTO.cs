using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_Employee_Salary_DetailsDTO
    {
        public long HRESD_Id { get; set; }
        public long HRES_Id { get; set; }
        public long HRMED_Id { get; set; }
         public long HRMED_Order { get; set; }
        public decimal? HRESD_Amount { get; set; }
     //   public decimal? HRES_ESIEmplr { get; set; }

        //
        public string HRMED_Name { get; set; }
        public string HRMED_EarnDedFlag { get; set; }
        public HR_Employee_Salary_DetailsDTO[] earningoutDTO { get; set; }
        public HR_Employee_Salary_DetailsDTO[] DeductionoutDTO { get; set; }

        public string HRES_Year { get; set; }
        public long HRME_Id { get; set; }
        public string HRES_Month { get; set; }
    }
}
