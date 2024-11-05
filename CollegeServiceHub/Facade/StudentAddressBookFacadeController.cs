using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeServiceHub.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class StudentAddressBookFacadeController : Controller
    {
        public StudentAddressBookInterface _inter;
        public StudentAddressBookFacadeController(StudentAddressBookInterface p)
        {
            _inter = p;
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

        [Route("loaddata")]
        public StudentAddressBookDTO loaddata([FromBody] StudentAddressBookDTO data)
        {
            return _inter.loaddata(data);
        }

        [Route("getcourse")]
        public StudentAddressBookDTO getcourse([FromBody] StudentAddressBookDTO data)
        {
            return _inter.getcourse(data);
        }

        [Route("getbranch")]
        public StudentAddressBookDTO getbranch([FromBody] StudentAddressBookDTO data)
        {
            return _inter.getbranch(data);
        }

        [Route("getsemester")]
        public StudentAddressBookDTO getsemester([FromBody] StudentAddressBookDTO data)
        {
            return _inter.getsemester(data);
        }

        [Route("onselectBranch")]
        public StudentAddressBookDTO onselectBranch([FromBody] StudentAddressBookDTO data)
        {
            return _inter.onselectBranch(data);
        }

        [Route("getsection")]
        public StudentAddressBookDTO getsection([FromBody] StudentAddressBookDTO data)
        {
            return _inter.getsection(data);
        }
        

        [Route("getstudent")]
        public StudentAddressBookDTO getstudent([FromBody] StudentAddressBookDTO data)
        {
            return _inter.getstudent(data);
        }

        [Route("Report")]
        public StudentAddressBookDTO Report([FromBody] StudentAddressBookDTO data)
        {
            return _inter.Report(data);
        }

        [Route("AddressBookFormat2")]
        public StudentAddressBookDTO AddressBookFormat2([FromBody] StudentAddressBookDTO data)
        {
            return _inter.AddressBookFormat2(data);
        }
    }
}
