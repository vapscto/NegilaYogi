using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class FilterEmployeeDataDTO
    {

        public long MI_Id { get; set; }
        public long LogInUserId { get; set; }

        public Array employeedetailList { get; set; }
        public Array groupTypedropdownlist { get; set; }
        public Array departmentdropdownlist { get; set; }
        public Array designationdropdownlist { get; set; }

    }
}
