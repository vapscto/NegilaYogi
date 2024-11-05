using System;
using System.Collections.Generic;
using IVRMUX.Delegates.com.vapstech.Alumni;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Alumni;
using DomainModel.Model;
using Microsoft.AspNetCore.Http;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Alumni
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class CLGAlumniMembershipController : Controller
    {
        CLGAlumniMembershipDelegate AlumniDelegates = new CLGAlumniMembershipDelegate();

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
     
        [Route("get_intial_data/{id:int}")]
        public CLGAlumniStudentDTO get_intial_data(int id)
        {
            CLGAlumniStudentDTO data = new CLGAlumniStudentDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            
            data.MI_Id = mid;
            return AlumniDelegates.get_intial_data(data);
        }

        [Route("Getstudentlist")]
        public CLGAlumniStudentDTO Getstudentlist([FromBody]CLGAlumniStudentDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //sddto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return AlumniDelegates.Getstudentlist(sddto);
        }

        [Route("Getstudentlistapp")]
        public CLGAlumniStudentDTO Getstudentlistapp([FromBody]CLGAlumniStudentDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //sddto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return AlumniDelegates.Getstudentlistapp(sddto);
        }

        [Route("checkstudent")]
        public CLGAlumniStudentDTO checkstudent([FromBody]CLGAlumniStudentDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //sddto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return AlumniDelegates.checkstudent(sddto);
        }

        [Route("aproovedata")]
        public CLGAlumniStudentDTO aproovedata([FromBody]CLGAlumniStudentDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //sddto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return AlumniDelegates.aproovedata(sddto);
        }


        [Route("getstudata")]
        public CLGAlumniStudentDTO getstudata([FromBody]CLGAlumniStudentDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            // sddto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));                 
            sddto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return AlumniDelegates.getstudata(sddto);
        }

        // POST api/<controller>
        [HttpPost]
        [Route("searchfilter")]
        public CLGAlumniStudentDTO searchfilter([FromBody]CLGAlumniStudentDTO student)
        {
            student.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return AlumniDelegates.searchfilter(student);
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


        [Route("savedata")]
        public CLGAlumniStudentDTO savedata([FromBody]CLGAlumniStudentDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            sddto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return AlumniDelegates.savedata(sddto);
        }

        [HttpPost]
        [Route("onchangecountry")]
        public CLGAlumniStudentDTO onchangecountry([FromBody]CLGAlumniStudentDTO student)
        {
            student.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return AlumniDelegates.onchangecountry(student);
        }
    }
}
