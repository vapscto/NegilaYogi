using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace PreadmissionDTOs.com.vaps.Portals.Employee
{
    public class EmployeeInvestmentDTO
    {
        public long HREID_Id

        { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long IMFY_Id { get; set; }
        public long HRMCVIA_Id

        { get; set; }
        public decimal? HREID_Amount

        { get; set; }
        // public string HRETDS_ChallanNo { get; set; }
        public bool HREID_ActiveFlg

        { get; set; }
        public long HREID_CreatedBy

        { get; set; }
        public long HREID_UpdatedBy

        { get; set; }
      
        public long UserId { get; set; }
        public Array leaveyeardropdown { get; set; }

        public string IMFY_FinancialYear { get; set; }
        public string HRMCVIA_SectionName { get; set; }

        //  public string hrme_address { get; set; }
         public Array allowancedropdown { get; set; }

        //  public Array empDetails { get; set; }
        //  public string HRME_PFAccNo { get; set; }
        //  public string HRME_FatherName { get; set; }
        //  public string HRME_PerCity { get; set; }
        // public string HRMDES_DesignationName { get; set; }
        //  public Array designation { get; set; }
        //  public string HRME_PANCardNo { get; set; }

          public string HRME_EmployeeFirstName { get; set; }

        //  public InstitutionDTO institutionDetails;
        //  public long? HRME_PerPincode { get; set; }

        public long roleId { get; set; }

      public Array emploanList { get; set; }
        public string retrunMsg { get; set; }
     
       public Array employeedropdown { get; set; }

      //  public Array masterloandropdown { get; set; }


      //  public string hrmE_EmployeeFirstName { get; set; }

      //  public string HRML_LoanType { get; set; }


     
      //  public Array modeOfPaymentdropdown { get; set; }
      //  //public HR_ConfigurationDTO configurationDetails { get; set; }
      //  public Master_NumberingDTO transnumconfigsettings { get; set; }
       public long LogInUserId { get; set; }
      //  //Academic Year
      //  public long ASMAY_Id { get; set; }
     public decimal? empGrossSal { get; set; }

    }
}
