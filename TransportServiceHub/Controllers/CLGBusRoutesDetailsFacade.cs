using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Transport;
using TransportServiceHub.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TransportServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class CLGBusRoutesDetailsFacade : Controller
    {

        public CLGBusRoutesDetailsInterface _inter;

        public CLGBusRoutesDetailsFacade(CLGBusRoutesDetailsInterface u)
        {
            _inter = u;
        }
        [Route("loaddata")]
        public CLGBusRoutesDetailsDTO loaddata([FromBody] CLGBusRoutesDetailsDTO data)
        {
            return _inter.loaddata(data);
        }[Route("getbranch")]
        public CLGBusRoutesDetailsDTO getbranch([FromBody] CLGBusRoutesDetailsDTO data)
        {
            return _inter.getbranch(data);
        }[Route("getsemester")]
        public CLGBusRoutesDetailsDTO getsemester([FromBody] CLGBusRoutesDetailsDTO data)
        {
            return _inter.getsemester(data);
        }

       
        [Route("getreport")]
        public CLGBusRoutesDetailsDTO getreport([FromBody] CLGBusRoutesDetailsDTO data)
        {
            return _inter.getreport(data);
        }
    }
}
