using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace PreadmissionDTOs.com.vaps.Portals.Employee
{
    public class EmployeeInvestmentothersDTO
    {
        public long HREIDO_Id{ get; set; }
        public bool HREID_ActiveFlg{ get; set; }
        public long HREID_UpdatedBy{ get; set; }
        public string IMFY_FinancialYear { get; set; }
        public string HRMCVIA_SectionName { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public Array emploanListgrid { get; set; }
        public long UserId { get; set; }
        public Array leaveyeardropdown { get; set; }
        public long LogInUserId { get; set; }
        public decimal? empGrossSal { get; set; }
        public long roleId { get; set; }
        public Array emploanList { get; set; }
        public string retrunMsg { get; set; }
        public Array employeedropdown { get; set; }
        public Array emploangrid { get; set; }

        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long IMFY_Id { get; set; }
        public long HRMCVIA_Id { get; set; }
        public decimal? HREIDO_RentPaid { get; set; }
        public bool HREIDO_ActiveFlg { get; set; }
        public string HREIDO_RentPaidEvidence { get; set; }
        public string HREIDO_NameOfLandLord { get; set; }
        public string HREIDO_NameEvidence { get; set; }
        public string HREIDO_LandLordAddress { get; set; }

        public string HREIDO_AddressEvidence { get; set; }
        public string HREIDO_LandLordPAN { get; set; }
        public string HREIDO_PANEvidence { get; set; }


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
        public long HREIDO_FinanceInst { get; set; }

        public string HREIDO_InstEvidence { get; set; }
        public string HREIDO_Employer { get; set; }
        public string HREIDO_EmpEvidence { get; set; }
        public string HREIDO_Others { get; set; }


        public string HREIDO_OthersEvidence { get; set; }
        public long HREIDO_CreatedBy
        { get; set; }
        public long HREIDO_UpdatedBy { get; set; }
    }
}
