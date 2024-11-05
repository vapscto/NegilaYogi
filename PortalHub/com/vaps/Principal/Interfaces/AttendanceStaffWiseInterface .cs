using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Portals.Principal;


namespace PortalHub.com.vaps.Principal.Interfaces
{
  public  interface AttendanceStaffWiseInterface
    {
        AttendanceStaffWiseDTO Getdetails(AttendanceStaffWiseDTO data);
        AttendanceStaffWiseDTO Getdepartment(AttendanceStaffWiseDTO data);
        AttendanceStaffWiseDTO get_designation(AttendanceStaffWiseDTO data);
        AttendanceStaffWiseDTO get_department(AttendanceStaffWiseDTO data);
        AttendanceStaffWiseDTO get_employee(AttendanceStaffWiseDTO data);
        
    }
}
