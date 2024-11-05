using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using TimeTableServiceHub.Interfaces;
using PreadmissionDTOs.com.vaps.TT;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeTableServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class CLGLabFacadeController : Controller
    {
        public CLGLabInterface _acd;
        public CLGLabFacadeController(CLGLabInterface acdm)
        {
            _acd = acdm;
        }
        [HttpPost]
        [Route("getalldetails")]
        public CLGLabDTO getalldetails([FromBody]CLGLabDTO data)
        {
            return _acd.getalldetails(data);
        }
        [HttpPost]
        [Route("editlab")]
        public CLGLabDTO editlab([FromBody]CLGLabDTO data)
        {
            return _acd.editlab(data);
        }


        [HttpPost]
        [Route("savedetail")]
        public CLGLabDTO savedetail([FromBody]CLGLabDTO data)
        {
          
            return _acd.savedetail(data);
          
        }
        [HttpPost]
        [Route("viewrecordspopup")]
        public CLGLabDTO viewrecordspopup([FromBody]CLGLabDTO data)
        {
            return _acd.viewrecordspopup(data);
        }

        
        [HttpPost]
        [Route("deactivate")]
        public CLGLabDTO deactivate([FromBody]CLGLabDTO data)
        {
            return _acd.deactivate(data);
        }
       
    }
}
