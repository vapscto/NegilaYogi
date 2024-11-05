using CommonLibrary;
using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Hostel
{
    public class HostelAllotForGuestDelegate
    {
        CommonDelegate<HostelAllotForGuest_DTO, HostelAllotForGuest_DTO> comm = new CommonDelegate<HostelAllotForGuest_DTO, HostelAllotForGuest_DTO>();
        public HostelAllotForGuest_DTO loaddata(HostelAllotForGuest_DTO data)
        {
            return comm.Post_Hostel(data, "HostelAllotForGuestFacade/loaddata");
        }
        public HostelAllotForGuest_DTO save(HostelAllotForGuest_DTO data)
        {
            return comm.Post_Hostel(data, "HostelAllotForGuestFacade/save");
        }
        public HostelAllotForGuest_DTO get_roomdetails(HostelAllotForGuest_DTO data)
        {
            return comm.Post_Hostel(data, "HostelAllotForGuestFacade/get_roomdetails");
        }
        public HostelAllotForGuest_DTO deactive(HostelAllotForGuest_DTO data)
        {
            return comm.Post_Hostel(data, "HostelAllotForGuestFacade/deactive");
        }
        public HostelAllotForGuest_DTO EditData(HostelAllotForGuest_DTO data)
        {
            return comm.Post_Hostel(data, "HostelAllotForGuestFacade/EditData");
        }
    }
}
