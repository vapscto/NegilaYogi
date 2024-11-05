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
    public class studentrequestfacade1 : Controller
    {
        public StudentRequestInterface _org;

        public studentrequestfacade1(StudentRequestInterface orga)
        {
            _org = orga;
        }

        [HttpPost]
        [Route("loaddata")]
        public Task<StudentRequestDTO> loaddata([FromBody] StudentRequestDTO data)
        {
            return _org.loaddata(data);
        }
        [Route("save")]
        public StudentRequestDTO save([FromBody] StudentRequestDTO data)
        {
            return _org.save(data);
        }
        [Route("edittab1")]
        public StudentRequestDTO edittab1([FromBody]  StudentRequestDTO data)
        {
            return _org.edittab1(data);
        }
        [Route("deactive")]
        public StudentRequestDTO deactive([FromBody] StudentRequestDTO data)
        {
            return _org.deactive(data);
        }
    }
}
