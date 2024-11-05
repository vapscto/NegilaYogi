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
    public class Naac_ICTController : Controller
    {
        public Naac_ICTDelegate _objdel = new Naac_ICTDelegate();
        // GET: api/<controller>
        [Route("loaddata/{id:int}")]
        public Naac_ICT_DTO loaddata(int id)
        {
            Naac_ICT_DTO data = new Naac_ICT_DTO();
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.loaddata(data);
        }

        [Route("savedata")]
        public Naac_ICT_DTO savedata([FromBody] Naac_ICT_DTO data)
        {

            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savedata(data);
        }
        [Route("getcomment")]
        public Naac_ICT_DTO getcomment([FromBody] Naac_ICT_DTO data)
        {

            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getcomment(data);
        }
        [Route("getfilecomment")]
        public Naac_ICT_DTO getfilecomment([FromBody] Naac_ICT_DTO data)
        {

            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getfilecomment(data);
        } [Route("savefilewisecomments")]
        public Naac_ICT_DTO savefilewisecomments([FromBody] Naac_ICT_DTO data)
        {

            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savefilewisecomments(data);
        }
        [Route("savemedicaldatawisecomments")]
        public Naac_ICT_DTO savemedicaldatawisecomments([FromBody] Naac_ICT_DTO data)
        {

            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savemedicaldatawisecomments(data);
        }

        [Route("editdata")]
        public Naac_ICT_DTO editdata([FromBody] Naac_ICT_DTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.editdata(data);
        }

        [Route("deactivRow")]
        public Naac_ICT_DTO deactivRow([FromBody] Naac_ICT_DTO data)
        {

            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.deactivRow(data);
        }
        [Route("viewuploadflies")]
        public Naac_ICT_DTO viewuploadflies([FromBody] Naac_ICT_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public Naac_ICT_DTO deleteuploadfile([FromBody] Naac_ICT_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.deleteuploadfile(data);
        }

    }
}
