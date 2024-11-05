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
    public class HostelVacateFacade : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public StudentVacantInterface _org;

        public HostelVacateFacade(StudentVacantInterface orga)
        {
            _org = orga;
        }


        [Route("binddata")]
        public Task<StudentVacantDTO> binddata([FromBody] StudentVacantDTO data)
        {
            return _org.loaddata(data);
        }

        [Route("edittab1")]
        public StudentVacantDTO edittab1([FromBody]  StudentVacantDTO data)
        {
            return _org.edittab1(data);
        }

        [Route("getalldetailsOnselectiontype")]
        public Task<StudentVacantDTO> getalldetailsOnselectiontype([FromBody] StudentVacantDTO data)
        {
            return _org.getalldetailsOnselectiontype(data);
        }

        [Route("get_studentDetail")]
        public Task<StudentVacantDTO> get_studentDetail([FromBody] StudentVacantDTO data)
        {
            return _org.get_studentDetail(data);
        }

        [Route("get_staffDetail")]
        public Task<StudentVacantDTO> get_staffDetail([FromBody] StudentVacantDTO data)
        {
            return _org.get_staffDetail(data);
        }
        [Route("get_guestDetail")]
        public StudentVacantDTO get_guestDetail([FromBody] StudentVacantDTO data)
        {
            return _org.get_guestDetail(data);
        }


    }
}
