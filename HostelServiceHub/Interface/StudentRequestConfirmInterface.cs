using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HostelServiceHub.Interface
{
   public interface StudentRequestConfirmInterface
    {
        Task<StudentRequestConfirm_DTO> loaddata(StudentRequestConfirm_DTO data);
        StudentRequestConfirm_DTO requestApproved(StudentRequestConfirm_DTO data);
        StudentRequestConfirm_DTO requestRejected(StudentRequestConfirm_DTO data);
        StudentRequestConfirm_DTO Ydeactive(StudentRequestConfirm_DTO data);
    }
}
