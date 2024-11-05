using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HostelServiceHub.Interface
{
   public interface CLGStudentRequestInterface
    {
        CLGStudentRequest_DTO save(CLGStudentRequest_DTO data);
        Task<CLGStudentRequest_DTO> loaddata(CLGStudentRequest_DTO data);
        CLGStudentRequest_DTO edittab1(CLGStudentRequest_DTO data);
        CLGStudentRequest_DTO roomdetails(CLGStudentRequest_DTO data);
        CLGStudentRequest_DTO Catgory(CLGStudentRequest_DTO data);
        CLGStudentRequest_DTO getPdetails(CLGStudentRequest_DTO data);
        CLGStudentRequest_DTO deactive(CLGStudentRequest_DTO data);
    }
}
