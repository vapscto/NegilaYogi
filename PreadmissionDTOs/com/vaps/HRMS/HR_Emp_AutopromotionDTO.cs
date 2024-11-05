using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_Emp_AutopromotionDTO
    {
     
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }

        public long LogInUserId { get; set; }

        public long roleId { get; set; }
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
        public Array employeedropdowndetails { get; set; }

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


      
    }

}
