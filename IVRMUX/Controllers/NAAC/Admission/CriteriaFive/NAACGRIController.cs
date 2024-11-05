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
    public class NAACGRIController : Controller
    {
        NAACGRIDelegate del = new NAACGRIDelegate();

        [Route("loaddata/{id:int}")]
        public NAACGRIDTO loaddata(int id)
        {
            NAACGRIDTO data = new NAACGRIDTO();
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = id;
            return del.loaddata(data);
        }


        [Route("loaddatamed/{id:int}")]
        public NAACGRIDTO loaddatamed(int id)
        {
            NAACGRIDTO data = new NAACGRIDTO();
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = id;
            return del.loaddatamed(data);
        }
        [Route("save")]
        public NAACGRIDTO save([FromBody]NAACGRIDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
          
            return del.save(data);
        }

        [Route("deactiveStudent")]
        public NAACGRIDTO deactiveStudent([FromBody] NAACGRIDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
         
            return del.deactiveStudent(data);
        }



        [Route("EditData")]
        public NAACGRIDTO EditData([FromBody]NAACGRIDTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.EditData(data);
        }
          [Route("viewuploadflies")]
        public NAACGRIDTO viewuploadflies([FromBody]NAACGRIDTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.viewuploadflies(data);
        }
          [Route("deleteuploadfile")]
        public NAACGRIDTO deleteuploadfile([FromBody]NAACGRIDTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deleteuploadfile(data);
        }
        [Route("savemedicaldatawisecomments")]
        public NAACGRIDTO savemedicaldatawisecomments([FromBody]NAACGRIDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savemedicaldatawisecomments(category);
        }
        [Route("getcomment")]
        public NAACGRIDTO getcomment([FromBody]NAACGRIDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getcomment(category);
        }
        [Route("getfilecomment")]
        public NAACGRIDTO getfilecomment([FromBody]NAACGRIDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getfilecomment(category);
        }
        [Route("savefilewisecomments")]
        public NAACGRIDTO savefilewisecomments([FromBody]NAACGRIDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savefilewisecomments(category);
        }

    }
}
