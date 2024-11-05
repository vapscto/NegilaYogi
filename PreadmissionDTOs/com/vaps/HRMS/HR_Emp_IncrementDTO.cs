using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_Emp_IncrementDTO : CommonParamDTO
    {    
        public long HREIC_Id { get; set; }
        public long MI_Id { get; set; }

        public long HRME_Id { get; set; }
        public DateTime HREIC_LastIncrementDate { get; set; }
        public DateTime HREIC_IncrementDueDate { get; set; }
        public DateTime HREIC_IncrementDate { get; set; }
        public bool? HREIC_ArrearApplicableFlg { get; set; }
        public bool? HREIC_ArrearGivenFlg { get; set; }   
        public long HREIC_ArrearMonths { get; set; }
        public bool HREIC_ActiveFlag { get; set; }
        public long HREIC_CreatedBy { get; set; }
        public long HREIC_UpdatedBy { get; set; }


        public long HREICED_Id { get; set; }
     


        public bool? HREICED_ActiveFlag { get; set; }
        public long HRMED_Id { get; set; }

        public decimal? HREICED_Amount { get; set; }
        public decimal? HREICED_Percentage { get; set; }

        public string HRMED_Name { get; set; }

        public decimal? HRESD_Amount { get; set; }

        public string HRMED_EarnDedFlag { get; set; }


    }

}
