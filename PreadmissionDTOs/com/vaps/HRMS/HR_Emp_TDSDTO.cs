using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_Emp_TDSDTO:CommonParamDTO
    {
        public long HRETDS_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long IMFY_Id { get; set; }
        public DateTime? HRETDS_DepositedDate { get; set; }
        public string HRETDS_BSRCode { get; set; }
        public string HRETDS_ChallanNo { get; set; }
        public bool HRETDS_ActiveFlg { get; set; }
        public long HRETDS_CreatedBy { get; set; }
        public long HRETDS_UpdatedBy { get; set; }
        public Array emploanList { get; set; }
        public string retrunMsg { get; set; }
        public long roleId { get; set; }

         public Array employeedropdown { get; set; }

  //  public Array masterloandropdown { get; set; }

 
             public string hrmE_EmployeeFirstName { get; set; }

     //   public string HRML_LoanType { get; set; }

        public string IMFY_FinancialYear { get; set; }
        public Array leaveyeardropdown { get; set; }
       // public Array modeOfPaymentdropdown { get; set; }
   // public HR_ConfigurationDTO configurationDetails { get; set; }
       // public Master_NumberingDTO transnumconfigsettings { get; set; }
        public long LogInUserId { get; set; }
        //Academic Year
         public decimal? HRETDS_TaxDeposited { get; set; }
        public decimal? empGrossSal { get; set; }

        }

}
