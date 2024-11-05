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
    public class NAACEncDevSchemeController : Controller
    {
        NAACEncDevSchemeDelegate del = new NAACEncDevSchemeDelegate();

        [Route("loaddata/{id:int}")]
        public NAACEncDevSchemeDTO loaddata(int id)
        {
            NAACEncDevSchemeDTO data = new NAACEncDevSchemeDTO();
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = id;
            return del.loaddata(data);
        }
        [Route("save")]
        public NAACEncDevSchemeDTO save([FromBody]NAACEncDevSchemeDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
          
            return del.save(data);
        }

        [Route("deactiveStudent")]
        public NAACEncDevSchemeDTO deactiveStudent([FromBody] NAACEncDevSchemeDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
          
            return del.deactiveStudent(data);
        }



        [Route("EditData")]
        public NAACEncDevSchemeDTO EditData([FromBody]NAACEncDevSchemeDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.EditData(data);
        }
          [Route("viewuploadflies")]
        public NAACEncDevSchemeDTO viewuploadflies([FromBody]NAACEncDevSchemeDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.viewuploadflies(data);
        }
          [Route("deleteuploadfile")]
        public NAACEncDevSchemeDTO deleteuploadfile([FromBody]NAACEncDevSchemeDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deleteuploadfile(data);
        }
        [Route("savemedicaldatawisecomments")]
        public NAACEncDevSchemeDTO savemedicaldatawisecomments([FromBody]NAACEncDevSchemeDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savemedicaldatawisecomments(category);
        }
        [Route("getcomment")]
        public NAACEncDevSchemeDTO getcomment([FromBody]NAACEncDevSchemeDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getcomment(category);
        }
        [Route("getfilecomment")]
        public NAACEncDevSchemeDTO getfilecomment([FromBody]NAACEncDevSchemeDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getfilecomment(category);
        }
        [Route("savefilewisecomments")]
        public NAACEncDevSchemeDTO savefilewisecomments([FromBody]NAACEncDevSchemeDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savefilewisecomments(category);
        }

    }
}
