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
    public class NaacExpAcaFacility441Controller : Controller
    {
        NaacExpAcaFacility441Delegate _Delobj = new NaacExpAcaFacility441Delegate();

        [Route("loaddata/{id:int}")]
        public NaacExpAcaFacility441DTO loaddata(int id)
        {
            NaacExpAcaFacility441DTO data = new NaacExpAcaFacility441DTO();
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _Delobj.loaddata(data);
        }
        [Route("save")]
        public NaacExpAcaFacility441DTO save([FromBody] NaacExpAcaFacility441DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Delobj.save(data);
        }
        [Route("getcomment")]
        public NaacExpAcaFacility441DTO getcomment([FromBody] NaacExpAcaFacility441DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Delobj.getcomment(data);
        }
        [Route("getfilecomment")]
        public NaacExpAcaFacility441DTO getfilecomment([FromBody] NaacExpAcaFacility441DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Delobj.getfilecomment(data);
        }
        [Route("savemedicaldatawisecomments")]
        public NaacExpAcaFacility441DTO savemedicaldatawisecomments([FromBody] NaacExpAcaFacility441DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Delobj.savemedicaldatawisecomments(data);
        }
        [Route("savefilewisecomments")]
        public NaacExpAcaFacility441DTO savefilewisecomments([FromBody] NaacExpAcaFacility441DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Delobj.savefilewisecomments(data);
        }

        [Route("deactiveStudent")]
        public NaacExpAcaFacility441DTO deactiveStudent([FromBody] NaacExpAcaFacility441DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Delobj.deactiveStudent(data);
        }

        [Route("EditData")]
        public NaacExpAcaFacility441DTO EditData([FromBody] NaacExpAcaFacility441DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Delobj.EditData(data);
        }

        [Route("viewuploadflies")]
        public NaacExpAcaFacility441DTO viewuploadflies([FromBody] NaacExpAcaFacility441DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Delobj.viewuploadflies(data);
        }

        [Route("deleteuploadfile")]
        public NaacExpAcaFacility441DTO deleteuploadfile([FromBody] NaacExpAcaFacility441DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Delobj.deleteuploadfile(data);
        }
    }
}
