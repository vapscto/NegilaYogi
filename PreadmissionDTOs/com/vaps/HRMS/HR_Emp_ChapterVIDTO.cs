using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_Emp_ChapterVIDTO
    {
        public long HRECVIA_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long IMFY_Id { get; set; }
        public long HRMCVIA_Id { get; set; }
        public string HRECVIA_SectionName { get; set; }
        public decimal? HRECVIA_Amount { get; set; }
        public bool HRECVIA_ActiveFlg { get; set; }
        public long HRECVIA_CreatedBy { get; set; }
        public long HRECVIA_UpdatedBy { get; set; }
        public Array emploanList { get; set; }
        public string retrunMsg { get; set; } 
        public long roleId { get; set; }

        public Array employeedropdown { get; set; }

        public Array masterloandropdown { get; set; }

 public string IMFY_FinancialYear { get; set; }
        public string hrmE_EmployeeFirstName { get; set; }
        public string HRMCVIA_SectionName { get; set; }
        public string HRML_LoanType { get; set; }
        public long HRMCVIA { get; set; }

        public Array leaveyeardropdown { get; set; }
        public Array modeOfPaymentdropdown { get; set; }
        public HR_ConfigurationDTO configurationDetails { get; set; }
        public Master_NumberingDTO transnumconfigsettings { get; set; }
        public long LogInUserId { get; set; }
        //Academic Year
        public long ASMAY_Id { get; set; }
        public decimal? empGrossSal { get; set; }

        }

}
