using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class MonthEndReportDTO
    {
        public long MI_Id { get; set; }
        public string retrunMsg { get; set; }
        public long roleId { get; set; }
        public string HRES_Year { get; set; }
        public string HRES_Month { get; set; }

        public Array monthdropdown { get; set; }
        public Array leaveyeardropdown { get; set; }
        public Array employeeDetails { get; set; }


        public int workingEmployee { get; set; }
        public int leftEmployee { get; set; }
        public int newEmployee { get; set; }

        public int salaryGenerated { get; set; }
        public int salaryslipGenerated { get; set; }


        //missing details of working employee
        public int missingPhoto { get; set; }
        public int missingEmailId { get; set; }
        public int missingContactNumber { get; set; }


        //missing details of left employee
        public int missingContactNumberleft { get; set; }
        public int missingEmailIdleft { get; set; }
        public int missingPhotoleft { get; set; }


        //missing details of new employee
        public int missingPhotonew { get; set; }
        public int missingEmailIdnew { get; set; }
        public int missingContactNumbernew { get; set; }

        public string monthendDate { get; set; }

        public InstitutionDTO institutionDetails { get; set; }

        public int salaryslipsent { get; set; }

        public int salaryslipsmssent { get; set; }

        public Array departmentdropdown { get; set; }
        public Array designationdropdown { get; set; }
        public Array groupTypedropdown { get; set; }
        public long?[] groupTypeIdList { get; set; }
        public long?[] hrmD_IdList { get; set; }
        public long?[] hrmdeS_IdList { get; set; }
        public long[] empid { get; set; }

        public Array smscount { get; set; }
        public Array emailcount { get; set; }
    }
}
