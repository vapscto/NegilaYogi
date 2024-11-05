using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HostelServiceHub.Interface
{
    public interface StaffRequestInterface
    {
        StaffRequestDTO save(StaffRequestDTO data);
        Task<StaffRequestDTO> loaddata(StaffRequestDTO data);
        StaffRequestDTO edittab1(StaffRequestDTO data);
        StaffRequestDTO deactive(StaffRequestDTO data);

    }
}
