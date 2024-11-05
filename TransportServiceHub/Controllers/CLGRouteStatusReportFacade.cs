﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;
using PreadmissionDTOs.com.vaps.Transport;
using TransportServiceHub.Interfaces;
namespace TransportServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class CLGRouteStatusReportFacade : Controller
    {
        public CLGRouteStatusReportInterface _feegrouppagee;
        // GET: api/values

        public CLGRouteStatusReportFacade(CLGRouteStatusReportInterface maspag)
        {
            _feegrouppagee = maspag;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }



        [HttpPost]

        [Route("Getreportdetails")]
        public RouteStatusReportDTO Getreportdetails([FromBody]RouteStatusReportDTO data)
        {
            return _feegrouppagee.Getreportdetails(data);
        }


        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("getdata/{id:int}")]
        public RouteStatusReportDTO getdata(int id)
        {
            return _feegrouppagee.getdata(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}