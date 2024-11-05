using CommonLibrary;
using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Hostel
{
    public class StaffRequestDelegate
    {

        CommonDelegate<StaffRequestDTO, StaffRequestDTO> comm = new CommonDelegate<StaffRequestDTO, StaffRequestDTO>();
        public StaffRequestDTO save(StaffRequestDTO data)
        {
            return comm.Post_Hostel(data, "Staff_RequestFacade/save/");
        }
        public StaffRequestDTO loaddata(StaffRequestDTO data)
        {
            return comm.Post_Hostel(data, "Staff_RequestFacade/loaddata/");
        }
        public StaffRequestDTO edittab1(StaffRequestDTO data)
        {
            return comm.Post_Hostel(data, "Staff_RequestFacade/edittab1");
        }
        public StaffRequestDTO deactive(StaffRequestDTO data)
        {
            return comm.Post_Hostel(data, "Staff_RequestFacade/deactive");
        }

    }
}
