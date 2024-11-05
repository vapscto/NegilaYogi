using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HostelServiceHub.Interface
{
   public interface HostelAllotForGuestInterface
    {
        HostelAllotForGuest_DTO loaddata(HostelAllotForGuest_DTO data);
        HostelAllotForGuest_DTO save(HostelAllotForGuest_DTO data);
        HostelAllotForGuest_DTO deactive(HostelAllotForGuest_DTO data);
        HostelAllotForGuest_DTO EditData(HostelAllotForGuest_DTO data);
        HostelAllotForGuest_DTO get_roomdetails(HostelAllotForGuest_DTO data);
    }
}
