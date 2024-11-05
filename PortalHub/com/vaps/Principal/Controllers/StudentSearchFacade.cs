using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.Principal.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Principal;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Principal.Controllers
{
    [Route("api/[controller]")]
    public class StudentSearchFacade : Controller
    {

        public StudentSearchInterface _org;
        public StudentSearchFacade(StudentSearchInterface org)
        {
            _org = org;
        }


        [Route("getalldetails")]
        public StudentSearchDTO getalldetails([FromBody] StudentSearchDTO data)
        {
            return _org.getdatastuacadgrp(data);
        }

        [Route("getstudentdetails")]
        public Task<StudentSearchDTO> getstudentdetails([FromBody] StudentSearchDTO data)
        {
            return _org.getstudentdetails(data);
        }
        [Route("GetStudentDetails1")]
        public Task<StudentSearchDTO> GetStudentDetails1([FromBody] StudentSearchDTO data)
        {
            return _org.GetStudentDetails1(data);
        }
         [Route("showsectionGrid")]
        public Task<StudentSearchDTO> showsectionGrid([FromBody] StudentSearchDTO data)
        {
            return _org.showsectionGrid(data);
        }

        [Route("GetStudentSearchByNameOrAdmno")]
        public StudentSearchDTO GetStudentSearchByNameOrAdmno([FromBody] StudentSearchDTO data)
        {
            return _org.GetStudentSearchByNameOrAdmno(data);
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
    }
}
