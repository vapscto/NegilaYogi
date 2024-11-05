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
    public class NaacBudget414Controller : Controller
    {
        // GET: api/<controller>
        NaacBudget_414_Delegate del = new NaacBudget_414_Delegate();

        [Route("loaddata/{id:int}")]
        public NaacBudget_414_DTO loaddata(int id)
        {
            NaacBudget_414_DTO data = new NaacBudget_414_DTO();
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
         
            return del.loaddata(data);
        }
        [Route("save")]
        public NaacBudget_414_DTO save([FromBody] NaacBudget_414_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.save(data);
        }
        [Route("getcomment")]
        public NaacBudget_414_DTO getcomment([FromBody] NaacBudget_414_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getcomment(data);
        }
        [Route("savefilewisecomments")]
        public NaacBudget_414_DTO savefilewisecomments([FromBody] NaacBudget_414_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savefilewisecomments(data);
        }
        [Route("savemedicaldatawisecomments")]
        public NaacBudget_414_DTO savemedicaldatawisecomments([FromBody] NaacBudget_414_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savemedicaldatawisecomments(data);
        }
        [Route("getfilecomment")]
        public NaacBudget_414_DTO getfilecomment([FromBody] NaacBudget_414_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getfilecomment(data);
        }
        [Route("EditData")]
        public NaacBudget_414_DTO EditData([FromBody] NaacBudget_414_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.EditData(data);
        }

        [Route("deactiveStudent")]
        public NaacBudget_414_DTO deactiveStudent([FromBody] NaacBudget_414_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.deactiveStudent(data);
        }

        [Route("viewuploadflies")]
        public NaacBudget_414_DTO viewuploadflies([FromBody] NaacBudget_414_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public NaacBudget_414_DTO deleteuploadfile([FromBody] NaacBudget_414_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.deleteuploadfile(data);
        }
        
    }
}
