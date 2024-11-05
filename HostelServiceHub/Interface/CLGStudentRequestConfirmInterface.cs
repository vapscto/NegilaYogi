using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HostelServiceHub.Interface
{
    public interface CLGStudentRequestConfirmInterface
    {
        Task<CLGStudentRequestConfirmDTO> loaddata(CLGStudentRequestConfirmDTO data);
        CLGStudentRequestConfirmDTO requestApproved(CLGStudentRequestConfirmDTO data);
        CLGStudentRequestConfirmDTO requestRejected(CLGStudentRequestConfirmDTO data);
        CLGStudentRequestConfirmDTO bedcapacity(CLGStudentRequestConfirmDTO data);
        CLGStudentRequestConfirmDTO Ydeactive(CLGStudentRequestConfirmDTO data);
    }
}
