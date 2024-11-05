using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.Principal
{
    public class AttendanceStaffWiseDTO : CommonParamDTO
    {
        public int EME_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long HRME_Id { get; set; }
        public long HRMDES_Id { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public Array employeedropdown { get; set; }
        public long?[] hrmgT_IdList { get; set; }
        public long?[] employeeTypeIdList { get; set; }
        public long?[] hrmD_IdList { get; set; }
        public long?[] hrmdeS_IdList { get; set; }
        public DateTime? Fromdate { get; set; }
        public DateTime? Todate { get; set; }
        public long ASMS_Id { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
      
        public long AMST_Id { get; set; }
        public Array StaffName { get; set; }
        public long roleId { get; set; }
        public string selectedRadiobtn { get; set; }
        public Array currentYear { get; set; }
        public Array employe { get; set; }
        public Array stafflist { get; set; }
        public string FOEST_IHalfLoginTime { get; set; }
        public string empFname { get; set; }
        public Array departmentdropdown { get; set; }
        public Array designationdropdown { get; set; }
        public DateTime? FOEP_PunchDate { get; set; }
        public string multipledep { get; set; }
        public string multipledes { get; set; }
        public string FOEPD_PunchTime { get; set; }
        public string ts { get; set; }
      
     public Array groupTypedropdown { get; set; }
        public string radiotype { get; set; }

        public string emailStatus { get; set; }


    }
}


