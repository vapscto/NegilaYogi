using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace PreadmissionDTOs.com.vaps.Portals.Employee
{
    public class EmployeeInvestmentSubsectionDTO
    {
        public long HREIDSS_Id


        { get; set; }
        public long HREID_Id
        { get; set; }
        public long HRMCVIAS_Id
        { get; set; }

        public decimal? HREIDSS_Amount


        { get; set; }
        // public string HRETDS_ChallanNo { get; set; }
        public bool HREIDSS_ActiveFlg


        { get; set; }
        public long HREIDSS_CreatedBy


        { get; set; }
        public long HREIDSS_UpdatedBy


        { get; set; }


        public long UserId { get; set; }
        public Array leaveyeardropdown { get; set; }
  
   
       // public string hrmE_EmployeeFirstName { get; set; }
      
      //  public string hrme_address { get; set; }
      ////  public Array employee_id { get; set; }

      //  public Array empDetails { get; set; }
      //  public string HRME_PFAccNo { get; set; }
      //  public string HRME_FatherName { get; set; }
      //  public string HRME_PerCity { get; set; }
      // public string HRMDES_DesignationName { get; set; }
      //  public Array designation { get; set; }
      //  public string HRME_PANCardNo { get; set; }

      //  public string HRME_EmployeeFirstName { get; set; }

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
        public long MI_Id { get; set; }
        public decimal? empGrossSal { get; set; }

    }
}
