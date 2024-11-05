﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class VikasaHallTicketReportFacadeController : Controller
    {
        private VikasaHallTicketReportInterface _inter;
        public VikasaHallTicketReportFacadeController(VikasaHallTicketReportInterface obj)
        {
            _inter = obj;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]

        [Route("getdetails")]
        public VikasaHallTicketReportDTO getdetails([FromBody] VikasaHallTicketReportDTO data)
        {
            return _inter.getdetails(data);
        }


        [Route("onselectAcdYear")]
        public VikasaHallTicketReportDTO onselectAcdYear([FromBody] VikasaHallTicketReportDTO data)
        {
            return _inter.onselectAcdYear(data);
        }

        [Route("onselectclass")]
        public VikasaHallTicketReportDTO onselectclass([FromBody] VikasaHallTicketReportDTO data)
        {
            return _inter.onselectclass(data);
        }

        [Route("onselectSection")]
        public VikasaHallTicketReportDTO onselectSection([FromBody] VikasaHallTicketReportDTO data)
        {
            return _inter.onselectSection(data);
        }

        [Route("report")]
        public VikasaHallTicketReportDTO report([FromBody] VikasaHallTicketReportDTO data)
        {
            return _inter.report(data);
        }

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