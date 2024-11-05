﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TimeTableServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.TT;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeTableServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ClassWiseTTFacade : Controller
    {
        public ClassWiseTTInterface _ttbreaktime;
        public ClassWiseTTFacade(ClassWiseTTInterface maspag)
        {
            _ttbreaktime = maspag;
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

        [Route("getdetails/{id:int}")]
        public TTClassWiseTTDTO getorgdet(int id)
        {
            return _ttbreaktime.getdetails(id);
        }
        [Route("getclass_catg")]
        public TTClassWiseTTDTO getclass_catg([FromBody] TTClassWiseTTDTO org)
        {
            return _ttbreaktime.getclass_catg(org);
        }
        [Route("getrpt")]
        public TTClassWiseTTDTO getrpt([FromBody] TTClassWiseTTDTO org)
        {
            return _ttbreaktime.getreport(org);
        }
       
    }
}
