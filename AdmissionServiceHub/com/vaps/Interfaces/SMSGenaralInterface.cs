using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;


//PreadmissionDTOs.com.vaps.admission


namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface SMSGenaralInterface
    {
        Task<SMSGenaralDTO> savedetail(SMSGenaralDTO data);
        Task<SMSGenaralDTO> GetStudentDetails(SMSGenaralDTO data);
        Task<SMSGenaralDTO> Getdetails(SMSGenaralDTO data);
        SMSGenaralDTO GetEmployeeDetailsByLeaveYearAndMonth(SMSGenaralDTO data);
        SMSGenaralDTO Getdepartment(SMSGenaralDTO data);
        SMSGenaralDTO get_designation(SMSGenaralDTO data);
        SMSGenaralDTO get_employee(SMSGenaralDTO data);
        SMSGenaralDTO Getexam(SMSGenaralDTO data);

    }
}
