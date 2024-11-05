using PreadmissionDTOs.com.vaps.Portals.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
   public interface CLG_PrincipleSMS_SendInterface
    {
        Task<SendSMSDTO> savedetail(SendSMSDTO data);
        Task<SendSMSDTO> GetStudentDetails(SendSMSDTO data);
        Task<SendSMSDTO> Getdetails(SendSMSDTO data);
        SendSMSDTO GetEmployeeDetailsByLeaveYearAndMonth(SendSMSDTO data);
        SendSMSDTO Getdepartment(SendSMSDTO data);
        SendSMSDTO get_designation(SendSMSDTO data);
        SendSMSDTO get_employee(SendSMSDTO data);
        SendSMSDTO getCourse(SendSMSDTO data);
        SendSMSDTO getBranch(SendSMSDTO data);
        SendSMSDTO getSemister(SendSMSDTO data);
    }
}
