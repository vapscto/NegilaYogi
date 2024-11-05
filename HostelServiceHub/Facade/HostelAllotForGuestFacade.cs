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
    public class HostelAllotForGuestFacade : Controller
    {
        public HostelAllotForGuestInterface inter;
        public HostelAllotForGuestFacade(HostelAllotForGuestInterface q)
        {
            inter = q;
        }
        [Route("loaddata")]
        public HostelAllotForGuest_DTO loaddata([FromBody] HostelAllotForGuest_DTO data)
        {
            return inter.loaddata(data);
        } [Route("get_roomdetails")]
        public HostelAllotForGuest_DTO get_roomdetails([FromBody] HostelAllotForGuest_DTO data)
        {
            return inter.get_roomdetails(data);
        }
        [Route("save")]
        public HostelAllotForGuest_DTO save([FromBody] HostelAllotForGuest_DTO data)
        {
            return inter.save(data);
        }
        [Route("deactive")]
        public HostelAllotForGuest_DTO deactive([FromBody] HostelAllotForGuest_DTO data)
        {
            return inter.deactive(data);
        }
        [Route("EditData")]
        public HostelAllotForGuest_DTO EditData([FromBody] HostelAllotForGuest_DTO data)
        {
            return inter.EditData(data);
        }
    }
}
