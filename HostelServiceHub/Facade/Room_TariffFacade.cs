using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HostelServiceHub.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Hostel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HostelServiceHub.Facade
{
    [Route("api/[controller]")]
    public class Room_TariffFacade : Controller
    {
        public Room_TariffInterface _Interface;
        public Room_TariffFacade(Room_TariffInterface parameter)
        {
            _Interface = parameter;
        }

        [Route("loaddata")]
        public Room_Tariff_DTO loaddata([FromBody] Room_Tariff_DTO data)
        {
            return _Interface.loaddata(data);
        }

        [Route("savedata")]
        public Room_Tariff_DTO savedata([FromBody] Room_Tariff_DTO data)
        {
            return _Interface.savedata(data);
        }

        [Route("editdata")]
        public Room_Tariff_DTO editdata([FromBody] Room_Tariff_DTO data)
        {
            return _Interface.editdata(data);
        }

        [Route("Ydeactive")]
        public Room_Tariff_DTO Ydeactive([FromBody]Room_Tariff_DTO data)
        {
            return _Interface.Ydeactive(data);
        }
        [Route("get_bedcount")]
        public Room_Tariff_DTO get_bedcount([FromBody]Room_Tariff_DTO data)
        {
            return _Interface.get_bedcount(data);
        }

    }
}
