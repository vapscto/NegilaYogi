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
    public class CLGHostelVacantFacadeController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public CLGHostelVacantInterface _org;

        public CLGHostelVacantFacadeController(CLGHostelVacantInterface orga)
        {
            _org = orga;
        }


        [Route("binddata")]
        public Task<CLGHostelVacantDTO> binddata([FromBody] CLGHostelVacantDTO data)
        {
            return _org.loaddata(data);
        }

        [Route("edittab1")]
        public CLGHostelVacantDTO edittab1([FromBody]  CLGHostelVacantDTO data)
        {
            return _org.edittab1(data);
        }

        [Route("getalldetailsOnselectiontype")]
        public Task<CLGHostelVacantDTO> getalldetailsOnselectiontype([FromBody] CLGHostelVacantDTO data)
        {
            return _org.getalldetailsOnselectiontype(data);
        }

        [Route("get_studentDetail")]
        public Task<CLGHostelVacantDTO> get_studentDetail([FromBody] CLGHostelVacantDTO data)
        {
            return _org.get_studentDetail(data);
        }

        [Route("get_staffDetail")]
        public Task<CLGHostelVacantDTO> get_staffDetail([FromBody] CLGHostelVacantDTO data)
        {
            return _org.get_staffDetail(data);
        }
        [Route("get_guestDetail")]
        public CLGHostelVacantDTO get_guestDetail([FromBody] CLGHostelVacantDTO data)
        {
            return _org.get_guestDetail(data);
        }


    }
}
