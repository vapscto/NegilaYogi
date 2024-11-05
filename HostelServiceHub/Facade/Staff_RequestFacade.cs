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
    public class Staff_RequestFacade : Controller
    {
        public StaffRequestInterface _org;

        public Staff_RequestFacade(StaffRequestInterface orga)
        {
            _org = orga;
        }

        [Route("loaddata")]
        public Task<StaffRequestDTO> loaddata([FromBody] StaffRequestDTO data)
        {
            return _org.loaddata(data);
        }
        [Route("save")]
        public StaffRequestDTO save([FromBody] StaffRequestDTO data)
        {
            return _org.save(data);
        }
        [Route("edittab1")]
        public StaffRequestDTO edittab1([FromBody]  StaffRequestDTO data)
        {
            return _org.edittab1(data);
        }
        [Route("deactive")]
        public StaffRequestDTO deactive([FromBody] StaffRequestDTO data)
        {
            return _org.deactive(data);
        }

    }
}
