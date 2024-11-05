using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using PortalHub.com.vaps.HOD.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Student;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.HOD.Controllers
{
    [Route("api/[controller]")]
    public class HODStudentSearchFacadeController : Controller
    {
        public HODStudentSearchInterface _org;
        public HODStudentSearchFacadeController(HODStudentSearchInterface org)
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

      
    }
}
