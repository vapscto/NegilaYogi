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
    public class NAACMasterSportsCAController : Controller
    {
        NAACMasterSportsCADelegate del = new NAACMasterSportsCADelegate();

        [Route("loaddata/{id:int}")]
        public NAACMasterSportsCADTO loaddata(int id)
        {
            NAACMasterSportsCADTO data = new NAACMasterSportsCADTO();
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = id;
            return del.loaddata(data);
        }
        [Route("save")]
        public NAACMasterSportsCADTO save([FromBody]NAACMasterSportsCADTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
           
            return del.save(data);
        }

        [Route("deactiveStudent")]
        public NAACMasterSportsCADTO deactiveStudent([FromBody] NAACMasterSportsCADTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            
            return del.deactiveStudent(data);
        }



        [Route("EditData")]
        public NAACMasterSportsCADTO EditData([FromBody]NAACMasterSportsCADTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.EditData(data);
        }
          [Route("viewuploadflies")]
        public NAACMasterSportsCADTO viewuploadflies([FromBody]NAACMasterSportsCADTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.viewuploadflies(data);
        }
          [Route("deleteuploadfile")]
        public NAACMasterSportsCADTO deleteuploadfile([FromBody]NAACMasterSportsCADTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deleteuploadfile(data);
        }
        [Route("savemedicaldatawisecomments")]
        public NAACMasterSportsCADTO savemedicaldatawisecomments([FromBody]NAACMasterSportsCADTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savemedicaldatawisecomments(category);
        }
        [Route("getcomment")]
        public NAACMasterSportsCADTO getcomment([FromBody]NAACMasterSportsCADTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getcomment(category);
        }
        [Route("getfilecomment")]
        public NAACMasterSportsCADTO getfilecomment([FromBody]NAACMasterSportsCADTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getfilecomment(category);
        }
        [Route("savefilewisecomments")]
        public NAACMasterSportsCADTO savefilewisecomments([FromBody]NAACMasterSportsCADTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savefilewisecomments(category);
        }

    }
}
