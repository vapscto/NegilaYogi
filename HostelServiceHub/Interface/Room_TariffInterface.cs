using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HostelServiceHub.Interface
{
   public interface Room_TariffInterface
    {
        Room_Tariff_DTO loaddata(Room_Tariff_DTO data);
        Room_Tariff_DTO savedata(Room_Tariff_DTO data);
        Room_Tariff_DTO editdata(Room_Tariff_DTO data);
        Room_Tariff_DTO Ydeactive(Room_Tariff_DTO data);
        Room_Tariff_DTO get_bedcount(Room_Tariff_DTO data);

    }
}
