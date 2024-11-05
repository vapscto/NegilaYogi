﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.TT;
using TimeTableServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeTableServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class TTMonthEndReportFacadeController : Controller
    {
        public TTMonthEndReportInterface _feetar;

        public TTMonthEndReportFacadeController(TTMonthEndReportInterface maspag)
        {
            _feetar = maspag;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
       
        [Route("getalldetails123")]
        public TTMonthEndReportDTO Getdet([FromBody] TTMonthEndReportDTO data)
        {
            return _feetar.getdata123(data);
        }

        [Route("getreport")]
        public Task<TTMonthEndReportDTO> getreport([FromBody] TTMonthEndReportDTO data)
        {
            return _feetar.getreport(data);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
