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
    public class ProgramMasterFacade : Controller
    {
        public ProgramMasterInterface _oei;
        public ProgramMasterFacade(ProgramMasterInterface oei)
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
        [Route("savedatalevel")]
        public OnlineProgramDTO savedatalevel([FromBody]OnlineProgramDTO data)
        {
            return _oei.savedatalevel(data);
        }
        [HttpPost]
        [Route("savedatatype")]
        public OnlineProgramDTO savedatatype([FromBody]OnlineProgramDTO data)
        {
            return _oei.savedatatype(data);
        }
        [HttpPost]
        [Route("editlevel")]
        public OnlineProgramDTO editlevel([FromBody]OnlineProgramDTO data)
        {
            return _oei.editlevel(data);
        }


        [HttpPost]
        [Route("deactivelevel")]
        public OnlineProgramDTO deactivelevel([FromBody]OnlineProgramDTO data)
        {
            return _oei.deactivelevel(data);
        }

        [HttpPost]
        [Route("edittype")]
        public OnlineProgramDTO edittype([FromBody]OnlineProgramDTO data)
        {
            return _oei.edittype(data);
        }

        [HttpPost]
        [Route("deactivetype")]
        public OnlineProgramDTO deactivetype([FromBody]OnlineProgramDTO data)
        {
            return _oei.deactivetype(data);
        }

    }
}
