using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlumniHub.Com.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Alumni;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AlumniHub.Com.Facade
{
    [Route("api/[controller]")]
    public class CLGAlumniMembershipFacade : Controller
    {

        public CLGAlumniApprovalInterface _Alumnimember;

        public CLGAlumniMembershipFacade(CLGAlumniApprovalInterface Alumnimember)
        {
            _Alumnimember = Alumnimember;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        
        [Route("Get_Intial_data")]
        public CLGAlumniStudentDTO Getdetails([FromBody] CLGAlumniStudentDTO CLGAlumniStudentDTO)//int IVRMM_Id
        {
            return _Alumnimember.Get_Intial_data(CLGAlumniStudentDTO);
        }

        [Route("getstudata")]
        public CLGAlumniStudentDTO getstudata([FromBody]CLGAlumniStudentDTO sddto)
        {
            return _Alumnimember.getstudata(sddto);
        }


        [Route("Getstudentlist")]
        public Task<CLGAlumniStudentDTO> Getstudentlist([FromBody]CLGAlumniStudentDTO sddto)
        {
            return _Alumnimember.Getstudentlist(sddto);
        }

        [Route("Getstudentlistapp")]
        public Task<CLGAlumniStudentDTO> Getstudentlistapp([FromBody]CLGAlumniStudentDTO sddto)
        {
            return _Alumnimember.Getstudentlistapp(sddto);
        }

        [Route("checkstudent")]
        public CLGAlumniStudentDTO checkstudent([FromBody]CLGAlumniStudentDTO sddto)
        {
            return _Alumnimember.checkstudent(sddto);
        }

        [Route("aproovedata")]
        public CLGAlumniStudentDTO aproovedata([FromBody]CLGAlumniStudentDTO sddto)
        {
            return _Alumnimember.aproovedata(sddto);
        }

        [Route("savedata")]
        public CLGAlumniStudentDTO savedata([FromBody]CLGAlumniStudentDTO sddto)
        {
            return _Alumnimember.savedata(sddto);
        }

        //[Route("onchangecountry")]
        //public CLGAlumniStudentDTO onchangecountry([FromBody]CLGAlumniStudentDTO sddto)
        //{
        //    return _Alumnimember.onchangecountry(sddto);
        //}

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        [Route("searchfilter")]
        public CLGAlumniStudentDTO searchfilter([FromBody]CLGAlumniStudentDTO sddto)
        {
            return _Alumnimember.searchfilter(sddto);
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
