using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;

using PreadmissionDTOs.com.vaps.OnlineProgram;
using NaacServiceHub.OnlineProgram.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.OnlineProgram.Facade
{
    [Route("api/[controller]")]
    public class ProgramDetailsFacade : Controller
    {
        public ProgramDetailsInterface _oei;
        public ProgramDetailsFacade(ProgramDetailsInterface oei)
        {
            _oei = oei;
        }
        [HttpPost]
        [Route("getloaddata")]
        public OnlineProgramDTO getloaddata([FromBody]OnlineProgramDTO data)
        {
            return _oei.getloaddata(data);
        }

        [HttpPost]
        [Route("savedetail")]
        public OnlineProgramDTO savedetail([FromBody]OnlineProgramDTO data)
        {
            return _oei.savedetail(data);
        }
        [HttpPost]
        [Route("getalldetailsviewrecords")]
        public OnlineProgramDTO getalldetailsviewrecords([FromBody]OnlineProgramDTO data)
        {
            return _oei.getalldetailsviewrecords(data);
        }
        [HttpPost]
        [Route("deleterecord")]
        public OnlineProgramDTO deleterecord([FromBody]OnlineProgramDTO data)
        {
            return _oei.deleterecord(data);
        }

    }
}
