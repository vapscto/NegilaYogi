using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_Employee_EarningsDeductionsDTO
    {
        public long HREED_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long HRMED_Id { get; set; }
        public decimal? HREED_Amount { get; set; }
        public string HREED_Percentage { get; set; }
        public bool? HREED_ActiveFlag { get; set; }

        //Head Name

        public string HRMED_Name { get; set; }

        public string HRMED_EarnDedFlag { get; set; }

        public string HRMED_AmountPercentFlag { get; set; }
        public decimal? HREED_ApplicableMaxValue { get; set; }
        public bool? HREED_MaxApplicableFlg { get; set; }


        public long roleId { get; set; }


        //dropdown Array list
        public Array employeedetailList { get; set; }
        public Array employeeTypedropdownlist { get; set; }
        public Array groupTypedropdownlist { get; set; }
        public Array departmentdropdownlist { get; set; }
        public Array designationdropdownlist { get; set; }
        public Array gradedropdownlist { get; set; }
        public Array genderdropdownlist { get; set; }

        //Type
        public string Type { get; set; }

        public Array earningList { get; set; }
        public Array detectionList { get; set; }

        public HR_Employee_EarningsDeductionsDTO[] EarningDTO { get; set; }
        public HR_Employee_EarningsDeductionsDTO[] DeductionDTO { get; set; }

        public HR_Employee_EarningsDeductionsDTO[] ArrearDTO { get; set; }
        public Array incrementDetails { get; set; }


        public Array employeeEarningsDeductionsDetails { get; set; }

        public HR_ConfigurationDTO configurationDetails { get; set; }

        public Array employeedropdowndetails { get; set; }
        public Array arrearList { get; set; }

        public Array grossList { get; set; }

        public string retrunMsg { get; set; }
        public string TabName { get; set; }


        public long?[] hrmgT_IdList { get; set; }
        public long?[] employeeTypeIdList { get; set; }
        public long?[] hrmD_IdList { get; set; }
        public long?[] hrmdeS_IdList { get; set; }
        public long?[] groupTypeIdList { get; set; }
        public long LogInUserId { get; set; }

        public Array departmentdropdown { get; set; }
        public Array designationdropdown { get; set; }
        public decimal? HRESD_Amount { get; set; }

        public long HRESD_Id { get; set; }

        public string hrmE_EmployeeFirstName { get; set; }

        public string HRMG_GradeName { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string HRMET_EmployeeType { get; set; }

        public string HRMGT_EmployeeGroupType { get; set; }

        public long HRMG_Id { get; set; }
        public long HRMET_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long HRMGT_Id { get; set; }
        public long HRMDES_Id { get; set; }



        public Array employeegrade { get; set; }
        public Array employeedropdown { get; set; }

        public Array employeedesig { get; set; }

        public Array employeedept { get; set; }

        public Array employeeemptype { get; set; }

        public Array employeeempgrouptype { get; set; }

        public string HRME_PHOTO { get; set; }

        public Array dropdownvalus { get; set; }

        public DateTime? HRME_DOB { get; set; }

        public DateTime? HRME_DOJ { get; set; }

        public DateTime? HRME_DOC { get; set; }

        public string HRME_EmployeeCode { get; set; }

        public string HRMG_PayScaleRange { get; set; }

        public  long HREIC_Id { get; set; }

        public bool HREIC_ArrearApplicableFlg { get; set; }

        public bool HREIC_ArrearGivenFlg { get; set; }
        public bool HREIC_ActiveFlag { get; set; }


        public DateTime HREIC_LastIncrementDate { get; set; }
        public DateTime HREIC_IncrementDueDate { get; set; }
        public DateTime HREIC_IncrementDate { get; set; }
        //public bool? HREIC_ArrearApplicableFlg { get; set; }
        //public bool? HREIC_ArrearGivenFlg { get; set; }
        public long HREIC_ArrearMonths { get; set; }
       // public bool HREIC_ActiveFlag { get; set; }
        public long HREIC_CreatedBy { get; set; }
        public long HREIC_UpdatedBy { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public long HREICED_Id { get; set; }


        public long HREICED_Percentage { get; set; }

        public long HREICED_Amount { get; set; }

        public bool HREICED_ActiveFlag { get; set; }

    }
}
