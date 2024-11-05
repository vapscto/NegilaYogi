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
    public class NAACInstShcrshipController : Controller
    {
        NAACInstShcrshipDelegate del = new NAACInstShcrshipDelegate();

        [Route("loaddata/{id:int}")]
        public NAACInstShcrshipDTO loaddata(int id)
        {
            NAACInstShcrshipDTO data = new NAACInstShcrshipDTO();
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata(data);
        }
        [Route("save")]
        public NAACInstShcrshipDTO save([FromBody]NAACInstShcrshipDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
          
            return del.save(data);
        }

        [Route("deactiveStudent")]
        public NAACInstShcrshipDTO deactiveStudent([FromBody] NAACInstShcrshipDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
        
            return del.deactiveStudent(data);
        }



        [Route("EditData")]
        public NAACInstShcrshipDTO EditData([FromBody]NAACInstShcrshipDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.EditData(data);
        }

        [Route("viewuploadflies")]
        public NAACInstShcrshipDTO viewuploadflies([FromBody]NAACInstShcrshipDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.viewuploadflies(data);
        }
             [Route("deleteuploadfile")]
        public NAACInstShcrshipDTO deleteuploadfile([FromBody]NAACInstShcrshipDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deleteuploadfile(data);
        }

        [Route("savemedicaldatawisecomments")]
        public NAACInstShcrshipDTO savemedicaldatawisecomments([FromBody]NAACInstShcrshipDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savemedicaldatawisecomments(category);
        }
        [Route("getcomment")]
        public NAACInstShcrshipDTO getcomment([FromBody]NAACInstShcrshipDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getcomment(category);
        }
        [Route("getfilecomment")]
        public NAACInstShcrshipDTO getfilecomment([FromBody]NAACInstShcrshipDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getfilecomment(category);
        }
        [Route("savefilewisecomments")]
        public NAACInstShcrshipDTO savefilewisecomments([FromBody]NAACInstShcrshipDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savefilewisecomments(category);
        }
    }
}
