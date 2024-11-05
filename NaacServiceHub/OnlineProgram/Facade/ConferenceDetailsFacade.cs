﻿using System;
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
    public class ConferenceDetailsFacade : Controller
    {
        public ConferenceDetailsInterface _oei;
        public ConferenceDetailsFacade(ConferenceDetailsInterface oei)
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
        [Route("Savedata")]
        public OnlineProgramDTO Savedata([FromBody]OnlineProgramDTO data)
        {
            return _oei.Savedata(data);
        }
        [HttpPost]
        [Route("getdetails")]
        public OnlineProgramDTO getdetails([FromBody]OnlineProgramDTO data)
        {
            return _oei.getdetails(data);
        }
        [HttpPost]
        [Route("delete")]
        public OnlineProgramDTO delete([FromBody]OnlineProgramDTO data)
        {
            return _oei.delete(data);
        }
        [Route("viewuploadflies")]
        public OnlineProgramDTO viewuploadflies([FromBody]OnlineProgramDTO data)
        {
            return _oei.viewuploadflies(data);
        }
        
    }
}
