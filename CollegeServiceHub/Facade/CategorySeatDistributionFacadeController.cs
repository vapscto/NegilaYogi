﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CollegeServiceHub.Interface;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class CategorySeatDistributionFacadeController : Controller
    {
        public CategorySeatDistributionInterface _inter;
        public CategorySeatDistributionFacadeController(CategorySeatDistributionInterface obj)
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
        public CategorySeatDistributionDTO getdetails([FromBody] CategorySeatDistributionDTO data)
        {
            return _inter.getdetails(data);
        }

        [Route("onreport")]
        public CategorySeatDistributionDTO onreport([FromBody] CategorySeatDistributionDTO data)
        {
            return _inter.onreport(data);
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
