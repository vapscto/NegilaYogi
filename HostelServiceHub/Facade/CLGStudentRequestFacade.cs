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
    public class CLGStudentRequestFacade : Controller
    {

        public CLGStudentRequestInterface _Interface;

        public CLGStudentRequestFacade(CLGStudentRequestInterface orga)
        {
            _Interface = orga;
        }

        [HttpPost]
        [Route("loaddata")]
        public Task<CLGStudentRequest_DTO> loaddata([FromBody] CLGStudentRequest_DTO data)
        {
            return _Interface.loaddata(data);
        }
        [Route("save")]
        public CLGStudentRequest_DTO save([FromBody] CLGStudentRequest_DTO data)
        {
            return _Interface.save(data);
        }
        [Route("edittab1")]
        public CLGStudentRequest_DTO edittab1([FromBody]  CLGStudentRequest_DTO data)
        {
            return _Interface.edittab1(data);
        }
        [Route("roomdetails")]
        public CLGStudentRequest_DTO roomdetails([FromBody]  CLGStudentRequest_DTO data)
        {
            return _Interface.roomdetails(data);
        }
        [Route("Catgory")]
        public CLGStudentRequest_DTO Catgory([FromBody]  CLGStudentRequest_DTO data)
        {
            return _Interface.Catgory(data);
        }
        [Route("getPdetails")]
        public CLGStudentRequest_DTO getPdetails([FromBody]  CLGStudentRequest_DTO data)
        {
            return _Interface.getPdetails(data);
        }
        [Route("deactive")]
        public CLGStudentRequest_DTO deactive([FromBody] CLGStudentRequest_DTO data)
        {
            return _Interface.deactive(data);
        }
    }
}
