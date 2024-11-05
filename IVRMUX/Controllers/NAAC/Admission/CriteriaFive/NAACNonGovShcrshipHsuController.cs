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
    public class NAACNonGovShcrshipHsuController : Controller
    {
        NAACNonGovShcrshipHsuDelegate del = new NAACNonGovShcrshipHsuDelegate();

        [Route("loaddata/{id:int}")]
        public NAACNonGovShcrshipHsuDTO loaddata(int id)
        {
            NAACNonGovShcrshipHsuDTO data = new NAACNonGovShcrshipHsuDTO();
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata(data);
        }
        [Route("save")]
        public NAACNonGovShcrshipHsuDTO save([FromBody]NAACNonGovShcrshipHsuDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
          
            return del.save(data);
        }

        [Route("deactiveStudent")]
        public NAACNonGovShcrshipHsuDTO deactiveStudent([FromBody] NAACNonGovShcrshipHsuDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
        
            return del.deactiveStudent(data);
        }



        [Route("EditData")]
        public NAACNonGovShcrshipHsuDTO EditData([FromBody]NAACNonGovShcrshipHsuDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.EditData(data);
        }

        [Route("viewuploadflies")]
        public NAACNonGovShcrshipHsuDTO viewuploadflies([FromBody]NAACNonGovShcrshipHsuDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.viewuploadflies(data);
        }
             [Route("deleteuploadfile")]
        public NAACNonGovShcrshipHsuDTO deleteuploadfile([FromBody]NAACNonGovShcrshipHsuDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deleteuploadfile(data);
        }
        [Route("savemedicaldatawisecomments")]
        public NAACNonGovShcrshipHsuDTO savemedicaldatawisecomments([FromBody]NAACNonGovShcrshipHsuDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savemedicaldatawisecomments(category);
        }
        [Route("getcomment")]
        public NAACNonGovShcrshipHsuDTO getcomment([FromBody]NAACNonGovShcrshipHsuDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getcomment(category);
        }
        [Route("getfilecomment")]
        public NAACNonGovShcrshipHsuDTO getfilecomment([FromBody]NAACNonGovShcrshipHsuDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getfilecomment(category);
        }
        [Route("savefilewisecomments")]
        public NAACNonGovShcrshipHsuDTO savefilewisecomments([FromBody]NAACNonGovShcrshipHsuDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savefilewisecomments(category);
        }

    }
}
