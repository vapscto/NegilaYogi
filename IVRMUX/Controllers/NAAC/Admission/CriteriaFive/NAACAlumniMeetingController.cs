using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Admission
{
    [Route("api/[controller]")]
    public class NAACAlumniMeetingController : Controller
    {
        NAACAlumniMeetingDelegate del = new NAACAlumniMeetingDelegate();

        [Route("loaddata/{id:int}")]
        public NAACAlumniMeetingDTO loaddata(int id)
        {
            NAACAlumniMeetingDTO data = new NAACAlumniMeetingDTO();
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = id;
            return del.loaddata(data);
        }
        [Route("save")]
        public NAACAlumniMeetingDTO save([FromBody]NAACAlumniMeetingDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
          
            return del.save(data);
        }

        [Route("deactiveStudent")]
        public NAACAlumniMeetingDTO deactiveStudent([FromBody] NAACAlumniMeetingDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
           
            return del.deactiveStudent(data);
        }



        [Route("EditData")]
        public NAACAlumniMeetingDTO EditData([FromBody]NAACAlumniMeetingDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.EditData(data);
        }
          [Route("viewuploadflies")]
        public NAACAlumniMeetingDTO viewuploadflies([FromBody]NAACAlumniMeetingDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.viewuploadflies(data);
        }
          [Route("deleteuploadfile")]
        public NAACAlumniMeetingDTO deleteuploadfile([FromBody]NAACAlumniMeetingDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deleteuploadfile(data);
        }
        [Route("savemedicaldatawisecomments")]
        public NAACAlumniMeetingDTO savemedicaldatawisecomments([FromBody]NAACAlumniMeetingDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savemedicaldatawisecomments(category);
        }
        [Route("getcomment")]
        public NAACAlumniMeetingDTO getcomment([FromBody]NAACAlumniMeetingDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getcomment(category);
        }
        [Route("getfilecomment")]
        public NAACAlumniMeetingDTO getfilecomment([FromBody]NAACAlumniMeetingDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getfilecomment(category);
        }
        [Route("savefilewisecomments")]
        public NAACAlumniMeetingDTO savefilewisecomments([FromBody]NAACAlumniMeetingDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savefilewisecomments(category);
        }

    }
}
