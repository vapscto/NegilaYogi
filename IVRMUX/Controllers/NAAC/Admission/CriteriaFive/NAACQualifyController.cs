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
    public class NAACQualifyController : Controller
    {
        NAACQualifyDelegate del = new NAACQualifyDelegate();

        [Route("loaddata/{id:int}")]
        public NAACQualifyDTO loaddata(int id)
        {
            NAACQualifyDTO data = new NAACQualifyDTO();
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = id;
            return del.loaddata(data);
        }
        [Route("save1")]
        public NAACQualifyDTO save1([FromBody]NAACQualifyDTO data)
        {
           
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.save1(data);
        }

        [Route("deactiveStudent1")]
        public NAACQualifyDTO deactiveStudent1([FromBody] NAACQualifyDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
           
            return del.deactiveStudent1(data);
        }



        [Route("EditData1")]
        public NAACQualifyDTO EditData1([FromBody]NAACQualifyDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.EditData1(data);
        }


        [Route("save")]
        public NAACQualifyDTO save([FromBody]NAACQualifyDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
           
            return del.save(data);
        }

        [Route("deactiveStudent")]
        public NAACQualifyDTO deactiveStudent([FromBody] NAACQualifyDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
           
            return del.deactiveStudent(data);
        }



        [Route("EditData")]
        public NAACQualifyDTO EditData([FromBody]NAACQualifyDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.EditData(data);
        }

        [Route("viewuploadflies")]
        public NAACQualifyDTO viewuploadflies([FromBody]NAACQualifyDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.viewuploadflies(data);
        }
             [Route("deleteuploadfile")]
        public NAACQualifyDTO deleteuploadfile([FromBody]NAACQualifyDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deleteuploadfile(data);
        }
        [Route("savemedicaldatawisecomments")]
        public NAACQualifyDTO savemedicaldatawisecomments([FromBody]NAACQualifyDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savemedicaldatawisecomments(category);
        }
        [Route("getcomment")]
        public NAACQualifyDTO getcomment([FromBody]NAACQualifyDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getcomment(category);
        }
        [Route("getfilecomment")]
        public NAACQualifyDTO getfilecomment([FromBody]NAACQualifyDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getfilecomment(category);
        }
        [Route("savefilewisecomments")]
        public NAACQualifyDTO savefilewisecomments([FromBody]NAACQualifyDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savefilewisecomments(category);
        }

    }
}
