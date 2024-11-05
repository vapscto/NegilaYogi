using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HostelServiceHub.Interface
{
   public interface StaffRequestConfirmInterface
    {
        Task<StaffRequestConfirm_DTO> loaddata(StaffRequestConfirm_DTO data);
        StaffRequestConfirm_DTO requestApproved(StaffRequestConfirm_DTO data);
        StaffRequestConfirm_DTO requestRejected(StaffRequestConfirm_DTO data);

    }
}
