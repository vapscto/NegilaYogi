using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class salaryUpdationDTO
    {
        public long MI_Id { get; set; }
        public string retrunMsg { get; set; }
        public long roleId { get; set; }

        public long? HRME_Id { get; set; }
       
        public Array employeedropdown { get; set; }
       
        public Array departmentdropdown { get; set; }
        public Array designationdropdown { get; set; }

        public Array employeeTypedropdown { get; set; }

        public Array groupTypedropdown { get; set; }

        public Array headdropdown { get; set; }

        public Array earningdropdown { get; set; }
        public Array detectiondropdown { get; set; }



        public long?[] groupTypeselected { get; set; }
        public long?[] departmentselected { get; set; }
        public long?[] designationselected { get; set; }
        public long?[] headselected { get; set; }

      
        //table data
        public string HRME_EmployeeCode { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }
       

       

        public HR_Employee_EarningsDeductionsDTO[] earningresult { get; set; }
        public HR_Employee_EarningsDeductionsDTO[] deductionresult { get; set; }

        public HR_Employee_Salary_DetailsDTO[] earningoutresult { get; set; }
        public HR_Employee_Salary_DetailsDTO[] deductionoutresult { get; set; }

        public salaryUpdationDTO[] selectedEmpdetails { get; set; }

        public salaryUpdationDTO[] selectedEmpssdetails { get; set; }

        public Array groupTypedropdownlist { get; set; }
        public Array departmentdropdownlist { get; set; }
        public Array designationdropdownlist { get; set; }
        public Array gradedropdownlist { get; set; }
        public Array employeeDetails { get; set; }

        public long LogInUserId { get; set; }

        public long?[] hrmgT_IdList { get; set; }
        public long?[] employeeTypeIdList { get; set; }
        public long?[] hrmD_IdList { get; set; }
        public long?[] hrmdeS_IdList { get; set; }
        public long?[] groupTypeIdList { get; set; }

        public long HRES_Id { get; set; }

        public Array configuartion { get; set; }
        public long IVRMSTAUL_Id { get; set; }
    }
}
