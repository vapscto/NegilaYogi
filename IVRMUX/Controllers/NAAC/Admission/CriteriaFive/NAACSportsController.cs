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
    public class NAACSportsController : Controller
    {
        NAACSportsDelegate del = new NAACSportsDelegate();

        [Route("loaddata/{id:int}")]
        public NAACSportsDTO loaddata(int id)
        {
            NAACSportsDTO data = new NAACSportsDTO();
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = id;
            return del.loaddata(data);
        }
       

        [Route("save")]
        public NAACSportsDTO save([FromBody]NAACSportsDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            
            return del.save(data);
        }

        [Route("deactiveStudent")]
        public NAACSportsDTO deactiveStudent([FromBody] NAACSportsDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
           
            return del.deactiveStudent(data);
        }



        [Route("EditData")]
        public NAACSportsDTO EditData([FromBody]NAACSportsDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.EditData(data);
        }

        [Route("viewuploadflies")]
        public NAACSportsDTO viewuploadflies([FromBody]NAACSportsDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.viewuploadflies(data);
        }

        [Route("deleteuploadfile")]
        public NAACSportsDTO deleteuploadfile([FromBody]NAACSportsDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deleteuploadfile(data);
        }

         [Route("get_course")]
        
        public NAACSportsDTO get_course([FromBody]NAACSportsDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.get_course(data);
        }
         [Route("get_branch")]
        public NAACSportsDTO get_branch([FromBody]NAACSportsDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.get_branch(data);
        }
         [Route("get_sems")]
        public NAACSportsDTO get_sems([FromBody]NAACSportsDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.get_sems(data);
        }
         [Route("get_section")]
        public NAACSportsDTO get_section([FromBody]NAACSportsDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.get_section(data);
        }
         [Route("GetStudentDetails")]
        public NAACSportsDTO GetStudentDetails([FromBody]NAACSportsDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.GetStudentDetails(data);
        }

        [Route("savemedicaldatawisecomments")]
        public NAACSportsDTO savemedicaldatawisecomments([FromBody]NAACSportsDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savemedicaldatawisecomments(category);
        }
        [Route("getcomment")]
        public NAACSportsDTO getcomment([FromBody]NAACSportsDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getcomment(category);
        }
        [Route("getfilecomment")]
        public NAACSportsDTO getfilecomment([FromBody]NAACSportsDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getfilecomment(category);
        }
        [Route("savefilewisecomments")]
        public NAACSportsDTO savefilewisecomments([FromBody]NAACSportsDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savefilewisecomments(category);
        }
    }
}
