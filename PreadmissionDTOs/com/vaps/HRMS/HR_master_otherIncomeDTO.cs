using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_master_otherIncomeDTO
    {

        public long HRMOI_Id

        { get; set; }
        public long MI_Id
        { get; set; }
        public string HRMOI_OtherIncomeName

        { get; set; }
        public bool HRMOI_OtherIncomeFlg

        { get; set; }
        public bool HRMOI_MaxLimitAplFlg

        { get; set; }
        public decimal? HRMOI_MaxLimit

        { get; set; }
        public bool HRMOI_ActiveFlg

        { get; set; }
        // public bool HRETDS_ActiveFlg { get; set; }
        public long HRMOI_CreatedBy
        { get; set; }
        public long HRMOI_UpdatedBy
        { get; set; }

        public Array bankdetailList { get; set; }
        public Array emploanList { get; set; }
        public string retrunMsg { get; set; }
        public long roleId { get; set; }

        public Array employeedropdown { get; set; }

        public Array masterloandropdown { get; set; }

 
        public string hrmE_EmployeeFirstName { get; set; }

        public string HRML_LoanType { get; set; }


        public Array leaveyeardropdown { get; set; }
        public Array modeOfPaymentdropdown { get; set; }
        public HR_ConfigurationDTO configurationDetails { get; set; }
        public Master_NumberingDTO transnumconfigsettings { get; set; }
        public long LogInUserId { get; set; }
        //Academic Year
        public long ASMAY_Id { get; set; }
        public decimal? empGrossSal { get; set; }

        public Array allowance { get; set; }

        }

}
