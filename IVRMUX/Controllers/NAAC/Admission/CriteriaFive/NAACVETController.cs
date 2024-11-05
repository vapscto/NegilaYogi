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
    public class NAACVETController : Controller
    {
        NAACVETDelegate del = new NAACVETDelegate();

        [Route("loaddata/{id:int}")]
        public NAACVETDTO loaddata(int id)
        {
            NAACVETDTO data = new NAACVETDTO();
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id =id;
            return del.loaddata(data);
        }
        [Route("save")]
        public NAACVETDTO save([FromBody]NAACVETDTO data)
        {
          
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.save(data);
        }

        [Route("deactiveStudent")]
        public NAACVETDTO deactiveStudent([FromBody] NAACVETDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
           
            return del.deactiveStudent(data);
        }



        [Route("EditData")]
        public NAACVETDTO EditData([FromBody]NAACVETDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.EditData(data);
        }
        [Route("viewuploadflies")]
        public NAACVETDTO viewuploadflies([FromBody]NAACVETDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public NAACVETDTO deleteuploadfile([FromBody]NAACVETDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deleteuploadfile(data);
        }
        [Route("savemedicaldatawisecomments")]
        public NAACVETDTO savemedicaldatawisecomments([FromBody]NAACVETDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savemedicaldatawisecomments(category);
        }
        [Route("getcomment")]
        public NAACVETDTO getcomment([FromBody]NAACVETDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getcomment(category);
        }
        [Route("getfilecomment")]
        public NAACVETDTO getfilecomment([FromBody]NAACVETDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getfilecomment(category);
        }
        [Route("savefilewisecomments")]
        public NAACVETDTO savefilewisecomments([FromBody]NAACVETDTO category)
        {

            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savefilewisecomments(category);
        }

    }
}
