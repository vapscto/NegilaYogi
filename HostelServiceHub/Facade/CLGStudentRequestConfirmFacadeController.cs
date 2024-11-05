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
    public class CLGStudentRequestConfirmFacadeController : Controller
    {
        public CLGStudentRequestConfirmInterface _Interface;
        public CLGStudentRequestConfirmFacadeController(CLGStudentRequestConfirmInterface parameter)
        {
            _Interface = parameter;
        }

        [Route("loaddata")]
        public Task<CLGStudentRequestConfirmDTO> loaddata([FromBody] CLGStudentRequestConfirmDTO data)
        {
            return _Interface.loaddata(data);
        }

        [Route("requestApproved")]
        public CLGStudentRequestConfirmDTO requestApproved([FromBody] CLGStudentRequestConfirmDTO data)
        {
            return _Interface.requestApproved(data);
        }

        [Route("requestRejected")]
        public CLGStudentRequestConfirmDTO requestRejected([FromBody] CLGStudentRequestConfirmDTO data)
        {
            return _Interface.requestRejected(data);
        }
        [Route("bedcapacity")]
        public CLGStudentRequestConfirmDTO bedcapacity([FromBody] CLGStudentRequestConfirmDTO data)
        {
            return _Interface.bedcapacity(data);
        }

        [Route("Ydeactive")]
        public CLGStudentRequestConfirmDTO Ydeactive([FromBody]CLGStudentRequestConfirmDTO data)
        {
            return _Interface.Ydeactive(data);
        }
    }
}
