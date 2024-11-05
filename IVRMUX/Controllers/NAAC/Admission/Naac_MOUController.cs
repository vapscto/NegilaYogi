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
    public class Naac_MOUController : Controller
    {
        Naac_MOUDelegate del = new Naac_MOUDelegate();

        [Route("loaddata/{id:int}")]
        public Naac_MOU_DTO loaddata(int id)
        {
            Naac_MOU_DTO data = new Naac_MOU_DTO();
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata(data);
        }
        [Route("save")]
        public Naac_MOU_DTO save([FromBody]Naac_MOU_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.save(data);
        }
        [Route("getcomment")]
        public Naac_MOU_DTO getcomment([FromBody]Naac_MOU_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getcomment(data);
        }
        [Route("getfilecomment")]
        public Naac_MOU_DTO getfilecomment([FromBody]Naac_MOU_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getfilecomment(data);
        }
        [Route("savefilewisecomments")]
        public Naac_MOU_DTO savefilewisecomments([FromBody]Naac_MOU_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.savefilewisecomments(data);
        }
        [Route("savemedicaldatawisecomments")]
        public Naac_MOU_DTO savemedicaldatawisecomments([FromBody]Naac_MOU_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.savemedicaldatawisecomments(data);
        }
        [Route("deactiveStudent")]
        public Naac_MOU_DTO deactiveStudent([FromBody] Naac_MOU_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.deactiveStudent(data);
        }
        [Route("viewuploadflies")]
        public Naac_MOU_DTO viewuploadflies([FromBody] Naac_MOU_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public Naac_MOU_DTO deleteuploadfile([FromBody] Naac_MOU_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.deleteuploadfile(data);
        }
        [Route("EditData")]
        public Naac_MOU_DTO EditData([FromBody]Naac_MOU_DTO category)
        {
            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //category.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.EditData(category);
        }
    }
}
