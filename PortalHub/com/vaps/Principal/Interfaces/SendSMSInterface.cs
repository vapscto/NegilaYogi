using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Portals.Principal;


namespace PortalHub.com.vaps.Principal.Interfaces
{
  public  interface SendSMSInterface
    {

       Task <SendSMSDTO> savedetail(SendSMSDTO data);
       Task<SendSMSDTO> GetStudentDetails(SendSMSDTO data);
       Task<SendSMSDTO> Getdetails(SendSMSDTO data);
       SendSMSDTO GetEmployeeDetailsByLeaveYearAndMonth(SendSMSDTO data);
       SendSMSDTO Getdepartment(SendSMSDTO data);
       SendSMSDTO get_designation(SendSMSDTO data);
       SendSMSDTO get_employee(SendSMSDTO data);
        
    }
}
