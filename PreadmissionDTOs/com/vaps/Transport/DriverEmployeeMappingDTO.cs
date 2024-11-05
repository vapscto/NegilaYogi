using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class DriverEmployeeMappingDTO
    {

        public long TRML_Id { get; set; }
        public long MI_Id { get; set; }

        public long TRDE_Id { get; set; }
        public long TRMD_Id { get; set; }

        public long HRME_Id { get; set; }
        public string TRMD_DriverName { get; set; }
        public string HRME_EmployeeFirstName { get; set; }

        public bool TRMD_ActiveFlg { get; set; }

        public Array employeedata { get; set; }
        public Array driverdata { get; set; }
        public Array savedata { get; set; }

        public Array edit { get; set; }
        public string message { get; set; }
        public bool retrunval { get; set; }
    }
}
