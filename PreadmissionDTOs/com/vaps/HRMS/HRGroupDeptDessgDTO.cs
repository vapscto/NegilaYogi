using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HRGroupDeptDessgDTO
    {
        public long HRGTDDS_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMGT_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long HRMDES_Id { get; set; }
        public bool HRGTDDS_ActiveFlg { get; set; }
        public long HRGTDDS_CreatedBy { get; set; }
        public long HRGTDDS_UpdatedBy { get; set; }
        public DateTime HRGTDDS_CreatedDate { get; set; }
        public DateTime HRGTDDS_UpdatedDate { get; set; }
        public Array groupTypedropdown { get; set; }
        public Array departmentdropdown { get; set; }
        public Array designationdropdown { get; set; }
        public Array gridviewdata { get; set; }
        public Array editlist { get; set; }
        public long[] designationids { get; set; }
        public long LogInUserId { get; set; }
        public string HRMGT_EmployeeGroupType { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public bool returnval { get; set; }
        public string msg { get; set; }
    }

}