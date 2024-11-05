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
    public class AlumniMembershipController : Controller
    {
        AlumniMembershipDelegate AlumniDelegates = new AlumniMembershipDelegate();

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
     
        [Route("get_intial_data/{id:int}")]
        public AlumniStudentDTO get_intial_data(int id)
        {
            AlumniStudentDTO data = new AlumniStudentDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            
            data.MI_Id = mid;
            return AlumniDelegates.get_intial_data(data);
        }

        [Route("Getstudentlist")]
        public AlumniStudentDTO Getstudentlist([FromBody]AlumniStudentDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //sddto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return AlumniDelegates.Getstudentlist(sddto);
        }

        [Route("Getstudentlistapp")]
        public AlumniStudentDTO Getstudentlistapp([FromBody]AlumniStudentDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //sddto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return AlumniDelegates.Getstudentlistapp(sddto);
        }

        [Route("checkstudent")]
        public AlumniStudentDTO checkstudent([FromBody]AlumniStudentDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //sddto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return AlumniDelegates.checkstudent(sddto);
        }

        [Route("aproovedata")]
        public AlumniStudentDTO aproovedata([FromBody]AlumniStudentDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //sddto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return AlumniDelegates.aproovedata(sddto);
        }


        [Route("getstudata")]
        public AlumniStudentDTO getstudata([FromBody]AlumniStudentDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            // sddto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));                 
            sddto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return AlumniDelegates.getstudata(sddto);
        }

        // POST api/<controller>
        [HttpPost]
        [Route("searchfilter")]
        public AlumniStudentDTO searchfilter([FromBody]AlumniStudentDTO student)
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
        public AlumniStudentDTO savedata([FromBody]AlumniStudentDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            sddto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            sddto.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return AlumniDelegates.svedata(sddto);
        }

        [Route("svedatanewalumni")]
        public AlumniStudentDTO svedatanewalumni([FromBody]AlumniStudentDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            sddto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            sddto.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return AlumniDelegates.svedatanewalumni(sddto);
        }

        [HttpPost]
        [Route("onchangecountry")]
        public AlumniStudentDTO onchangecountry([FromBody]AlumniStudentDTO student)
        {
            student.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return AlumniDelegates.onchangecountry(student);
        }

        [Route("onchangedistrict")]
        public AlumniStudentDTO onchangedistrict([FromBody]AlumniStudentDTO student)
        {
            student.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return AlumniDelegates.onchangedistrict(student);
        }


        [Route("viewData")]
        public AlumniStudentDTO viewData([FromBody]AlumniStudentDTO student)
        {
            student.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return AlumniDelegates.viewData(student);
        }
        [Route("onchangestate")]
        public AlumniStudentDTO onchangestate([FromBody]AlumniStudentDTO student)
        {
            student.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return AlumniDelegates.onchangestate(student);
        }

         [Route("deactive")]
        public AlumniStudentDTO deactive([FromBody]AlumniStudentDTO student)
        {
            student.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return AlumniDelegates.deactive(student);
        }
        //Akash
        [Route("EditAlumniHomepages")]
        public AlumniStudentDTO EditMasterQuestions([FromBody] AlumniStudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return AlumniDelegates.EditAlumniHomepages(data);
        }

        [Route("AlumniHomepageActiveDeactives")]
        public AlumniStudentDTO AlumniHomepageActiveDeactives([FromBody] AlumniStudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return AlumniDelegates.AlumniHomepageActiveDeactives(data);
        }

    }
}
