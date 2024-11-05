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
    public class NAACCompExamsController : Controller
    {
        NAACCompExamsDelegate del = new NAACCompExamsDelegate();

        [Route("loaddata/{id:int}")]
        public NAACCompExamsDTO loaddata(int id)
        {
            NAACCompExamsDTO data = new NAACCompExamsDTO();
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata(data);
        }
        [Route("save")]
        public NAACCompExamsDTO save([FromBody]NAACCompExamsDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
           
            return del.save(data);
        }

        [Route("deactiveStudent")]
        public NAACCompExamsDTO deactiveStudent([FromBody] NAACCompExamsDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
           
            return del.deactiveStudent(data);
        }



        [Route("EditData")]
        public NAACCompExamsDTO EditData([FromBody]NAACCompExamsDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.EditData(data);
        }

        [Route("viewuploadflies")]
        public NAACCompExamsDTO viewuploadflies([FromBody]NAACCompExamsDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.viewuploadflies(data);
        }

        [Route("deleteuploadfile")]
        public NAACCompExamsDTO deleteuploadfile([FromBody]NAACCompExamsDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.deleteuploadfile(data);
        }


        [Route("savemedicaldatawisecomments")]
        public NAACCompExamsDTO savemedicaldatawisecomments([FromBody]NAACCompExamsDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savemedicaldatawisecomments(category);
        }
        [Route("getcomment")]
        public NAACCompExamsDTO getcomment([FromBody]NAACCompExamsDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getcomment(category);
        }
        [Route("getfilecomment")]
        public NAACCompExamsDTO getfilecomment([FromBody]NAACCompExamsDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getfilecomment(category);
        }
        [Route("savefilewisecomments")]
        public NAACCompExamsDTO savefilewisecomments([FromBody]NAACCompExamsDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savefilewisecomments(category);
        }

    }
}
