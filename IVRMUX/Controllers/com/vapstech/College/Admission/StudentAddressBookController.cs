using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Admission
{
    [Route("api/[controller]")]
    public class StudentAddressBookController : Controller
    {
        StudentAddressBookDelegate del = new StudentAddressBookDelegate();
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

        [Route("loaddata/{id:int}")]
        public StudentAddressBookDTO loaddata(int id)
        {
            StudentAddressBookDTO data = new StudentAddressBookDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.loaddata(data);
        }
        [Route("getcourse")]
        public StudentAddressBookDTO getcourse([FromBody] StudentAddressBookDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getcourse(data);
        }
        [Route("getbranch")]
        public StudentAddressBookDTO getbranch([FromBody] StudentAddressBookDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getbranch(data);
        }

        [Route("getsemester")]
        public StudentAddressBookDTO getsemester([FromBody] StudentAddressBookDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getsemester(data);
        }

        [Route("onselectBranch")]
        public StudentAddressBookDTO onselectBranch([FromBody] StudentAddressBookDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.onselectBranch(data);
        }

        [Route("getsection")]
        public StudentAddressBookDTO getsection([FromBody] StudentAddressBookDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getsection(data);
        }

        [Route("getstudent")]
        public StudentAddressBookDTO getstudent([FromBody] StudentAddressBookDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getstudent(data);
        }

        [Route("Report")]
        public StudentAddressBookDTO Report([FromBody] StudentAddressBookDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.Report(data);
        }

        [Route("AddressBookFormat2")]
        public StudentAddressBookDTO AddressBookFormat2([FromBody] StudentAddressBookDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.AddressBookFormat2(data);
        }
    }
}
