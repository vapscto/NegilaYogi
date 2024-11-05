
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

//PreadmissionDTOs.com.vaps.admission


namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface General_SendSMSInterface
    {
        Task<General_SendSMSDTO> savedetail(General_SendSMSDTO data);
        Task<General_SendSMSDTO> GetStudentDetails(General_SendSMSDTO data);
        Task<General_SendSMSDTO> Getdetails(General_SendSMSDTO data);
        General_SendSMSDTO GetEmployeeDetailsByLeaveYearAndMonth(General_SendSMSDTO data);
        General_SendSMSDTO Getdepartment(General_SendSMSDTO data);
        General_SendSMSDTO get_designation(General_SendSMSDTO data);
        General_SendSMSDTO get_employee(General_SendSMSDTO data);
        General_SendSMSDTO Getexam(General_SendSMSDTO data);

        Task<General_SendSMSDTO> SrkvsSerach(General_SendSMSDTO data);
    }
}
