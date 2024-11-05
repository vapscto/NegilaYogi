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
    public class NAACHrEducationController : Controller
    {
        NAACHrEducationDelegate del = new NAACHrEducationDelegate();

        [Route("loaddata/{id:int}")]
        public NAACHrEducationDTO loaddata(int id)
        {
            NAACHrEducationDTO data = new NAACHrEducationDTO();
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = id;
            return del.loaddata(data);
        }
        [Route("save")]
        public NAACHrEducationDTO save([FromBody]NAACHrEducationDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
          
            return del.save(data);
        }

        [Route("deactiveStudent")]
        public NAACHrEducationDTO deactiveStudent([FromBody] NAACHrEducationDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
           
            return del.deactiveStudent(data);
        }



        [Route("EditData")]
        public NAACHrEducationDTO EditData([FromBody]NAACHrEducationDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.EditData(data);
        }
        [Route("viewuploadflies")]
        public NAACHrEducationDTO viewuploadflies([FromBody]NAACHrEducationDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public NAACHrEducationDTO deleteuploadfile([FromBody]NAACHrEducationDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deleteuploadfile(data);
        }
        [Route("get_course")]
        public NAACHrEducationDTO get_course([FromBody]NAACHrEducationDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.get_course(data);
        }
         [Route("get_branch")]
        public NAACHrEducationDTO get_branch([FromBody]NAACHrEducationDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.get_branch(data);
        }

        [Route("savemedicaldatawisecomments")]
        public NAACHrEducationDTO savemedicaldatawisecomments([FromBody]NAACHrEducationDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savemedicaldatawisecomments(category);
        }
        [Route("getcomment")]
        public NAACHrEducationDTO getcomment([FromBody]NAACHrEducationDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getcomment(category);
        }
        [Route("getfilecomment")]
        public NAACHrEducationDTO getfilecomment([FromBody]NAACHrEducationDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getfilecomment(category);
        }
        [Route("savefilewisecomments")]
        public NAACHrEducationDTO savefilewisecomments([FromBody]NAACHrEducationDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savefilewisecomments(category);
        }
    }
}
