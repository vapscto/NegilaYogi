using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace PreadmissionDTOs.com.vaps.Portals.Employee
{
    public class Employee12BBDTO
    {
        public string finyear { get; set; } 
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long UserId { get; set; }
        public Array leaveyeardropdown { get; set; }
        public long HRME_Id { get; set; }
        public string HRMLY_LeaveYear { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string hrme_address { get; set; }
        public Array employee_id { get; set; }
        public Array empDetails { get; set; }
        public string HRME_PFAccNo { get; set; }
        public string HRME_FatherName { get; set; }
        public string HRME_PerCity { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public Array designation { get; set; }
        public Array investmentdetails { get; set; }
        public string HRME_PANCardNo { get; set; }

        public decimal? HREIDO_RentPaid { get; set; }
        public string HREIDO_RentPaidEvidence { get; set; }

        public string HREIDO_NameOfLandLord { get; set; }
        public string HREIDO_NameEvidence { get; set; }
        public string HREIDO_LandLordAddress { get; set; }

        public string HREIDO_AddressEvidence { get; set; }
        public string HREIDO_LandLordPAN { get; set; }
        public string HREIDO_PANEvidence { get; set; }

        public long HRMCVIA_Id { get; set; }
        public decimal? HREIDO_TravelConcession { get; set; }
        public string HREIDO_ConcessionEvidence { get; set; }
        public decimal? HREIDO_InterestPaid { get; set; }

        public string HREIDO_InterestEvidence { get; set; }
        public string HREIDO_LenderName { get; set; }
        public string HREIDO_LNameEvidence { get; set; }
        public string HREIDO_LenderAddress { get; set; }

        public string HREIDO_LAddressEvidence { get; set; }
        public string HREIDO_LenderPAN { get; set; }
        public string HREIDO_LPANEvidence { get; set; }

        public string HRMCVIA_SectionName { get; set; }
        public string HREIDO_Employer { get; set; }
        public decimal? HREID_Amount { get; set; }
        public string HREIDO_InstEvidence { get; set; }
        public long? HREIDO_FinanceInst { get; set; }
        public string HREIDO_EmpEvidence { get; set; }

        public string HREIDO_Others { get; set; }

        public string HREIDO_OthersEvidence { get; set; }
         
        public decimal? HRECVIA_Amount { get; set;}

        public Array chapterlist { get; set; }

        public long IMFY_Id { get; set; }

        public Array chapterlist80E { get; set; }
    }
}
