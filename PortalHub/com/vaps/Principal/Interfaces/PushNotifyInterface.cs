using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Portals.Principal;


namespace PortalHub.com.vaps.Principal.Interfaces
{
  public  interface PushNotifyInterface
    {

       Task <PushNotifyDTO> savedetail(PushNotifyDTO data);
       Task<PushNotifyDTO> GetStudentDetails(PushNotifyDTO data);
       Task<PushNotifyDTO> Getdetails(PushNotifyDTO data);
       PushNotifyDTO GetEmployeeDetailsByLeaveYearAndMonth(PushNotifyDTO data);
       PushNotifyDTO Getdepartment(PushNotifyDTO data);
       PushNotifyDTO get_designation(PushNotifyDTO data);
       PushNotifyDTO get_employee(PushNotifyDTO data);
        
    }
}
