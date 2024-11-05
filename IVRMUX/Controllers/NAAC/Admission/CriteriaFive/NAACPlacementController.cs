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
    public class NAACPlacementController : Controller
    {
        NAACPlacementDelegate del = new NAACPlacementDelegate();

        [Route("loaddata/{id:int}")]
        public NAACPlacementDTO loaddata(int id)
        {
            NAACPlacementDTO data = new NAACPlacementDTO();
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata(data);
        }
        [Route("save")]
        public NAACPlacementDTO save([FromBody]NAACPlacementDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
           
            return del.save(data);
        }

        [Route("deactiveStudent")]
        public NAACPlacementDTO deactiveStudent([FromBody] NAACPlacementDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
           
            return del.deactiveStudent(data);
        }



        [Route("EditData")]
        public NAACPlacementDTO EditData([FromBody]NAACPlacementDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.EditData(data);
        }
        [Route("viewuploadflies")]
        public NAACPlacementDTO viewuploadflies([FromBody]NAACPlacementDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public NAACPlacementDTO deleteuploadfile([FromBody]NAACPlacementDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deleteuploadfile(data);
        }
        [Route("get_course")]
        public NAACPlacementDTO get_course([FromBody]NAACPlacementDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.get_course(data);
        }
         [Route("get_branch")]
        public NAACPlacementDTO get_branch([FromBody]NAACPlacementDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.get_branch(data);
        }

        [Route("savemedicaldatawisecomments")]
        public NAACPlacementDTO savemedicaldatawisecomments([FromBody]NAACPlacementDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savemedicaldatawisecomments(category);
        }
        [Route("getcomment")]
        public NAACPlacementDTO getcomment([FromBody]NAACPlacementDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getcomment(category);
        }
        [Route("getfilecomment")]
        public NAACPlacementDTO getfilecomment([FromBody]NAACPlacementDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getfilecomment(category);
        }
        [Route("savefilewisecomments")]
        public NAACPlacementDTO savefilewisecomments([FromBody]NAACPlacementDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savefilewisecomments(category);
        }
    }
}
