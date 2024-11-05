using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class EmployeeSalaryIncreementProcessDTO
    {
        public long MI_Id { get; set; }
        public long roleId { get; set; }
        public long HREIC_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime HREIC_LastIncrementDate { get; set; }
        public DateTime HREIC_IncrementDueDate { get; set; }
        public DateTime HREIC_IncrementDate { get; set; }
        public DateTime Fromdate { get; set; }
        public DateTime Todate { get; set; }
        public bool HREIC_ArrearApplicableFlg { get; set; }
        public bool HREIC_ArrearGivenFlg { get; set; }
        public long HREIC_ArrearMonths { get; set; }
        public long UserId { get; set; }
        public bool HREIC_ActiveFlag { get; set; }
        public bool HREICED_Id { get; set; }
        public long HRMED_Id { get; set; }
        public decimal HREICED_Amount { get; set; }
        public string HREICED_Percentage { get; set; }
        public bool HREICED_ActiveFlag { get; set; }
        public string retrunMsg { get; set; }
        public string Status { get; set; }
        public Array monthdropdown { get; set; }
        public Array griddata { get; set; }
        public Array Reportdata { get; set; }
        public Array employeelist { get; set; }
        public Array earningdeductiontype { get; set; }
        public Array getempsdetails { get; set; }
        public Array leaveyeardropdown { get; set; }
        public string Type { get; set; }
        public string Option { get; set; }
        public long Month { get; set; }
        public long Year { get; set; }
        public long selected_hrmeID { get; set; }
        public EmployeeSalaryIncreementProcessDTO[] employee { get; set; }
    }
}
