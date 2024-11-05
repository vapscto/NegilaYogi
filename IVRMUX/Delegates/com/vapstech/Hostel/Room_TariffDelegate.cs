using CommonLibrary;
using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Hostel
{
    public class Room_TariffDelegate
    {

        CommonDelegate<Room_Tariff_DTO, Room_Tariff_DTO> _commnbranch = new CommonDelegate<Room_Tariff_DTO, Room_Tariff_DTO>();

        public Room_Tariff_DTO loaddata(Room_Tariff_DTO data)
        {
            return _commnbranch.Post_Hostel(data, "Room_TariffFacade/loaddata/");
        }
        public Room_Tariff_DTO savedata(Room_Tariff_DTO data)
        {
            return _commnbranch.Post_Hostel(data, "Room_TariffFacade/savedata/");
        }
        public Room_Tariff_DTO editdata(Room_Tariff_DTO data)
        {
            return _commnbranch.Post_Hostel(data, "Room_TariffFacade/editdata/");
        }
        public Room_Tariff_DTO Ydeactive(Room_Tariff_DTO data)
        {
            return _commnbranch.Post_Hostel(data, "Room_TariffFacade/Ydeactive/");
        }
        public Room_Tariff_DTO get_bedcount(Room_Tariff_DTO data)
        {
            return _commnbranch.Post_Hostel(data, "Room_TariffFacade/get_bedcount/");
        }

    }
}
