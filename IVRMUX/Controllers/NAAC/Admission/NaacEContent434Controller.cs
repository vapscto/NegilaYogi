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
    public class NaacEContent434Controller : Controller
    {
        public NAAC_EContent_434_Delegate _Delobj = new NAAC_EContent_434_Delegate();

        [Route("loaddata/{id:int}")]
        public NAAC_AC_434_EContent_DTO loaddata(int id)
        {
            NAAC_AC_434_EContent_DTO data = new NAAC_AC_434_EContent_DTO();
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _Delobj.loaddata(data);
        }

        [Route("savedata")]
        public NAAC_AC_434_EContent_DTO savedata([FromBody]NAAC_AC_434_EContent_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _Delobj.savedata(data);
        }

        [Route("getcomment")]
        public NAAC_AC_434_EContent_DTO getcomment([FromBody]NAAC_AC_434_EContent_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _Delobj.getcomment(data);
        }
        

        [Route("getfilecomment")]
        public NAAC_AC_434_EContent_DTO getfilecomment([FromBody]NAAC_AC_434_EContent_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _Delobj.getfilecomment(data);
        }
        

        [Route("savefilewisecomments")]
        public NAAC_AC_434_EContent_DTO savefilewisecomments([FromBody]NAAC_AC_434_EContent_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _Delobj.savefilewisecomments(data);
        }
        

        [Route("savemedicaldatawisecomments")]
        public NAAC_AC_434_EContent_DTO savemedicaldatawisecomments([FromBody]NAAC_AC_434_EContent_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _Delobj.savemedicaldatawisecomments(data);
        }

        [Route("editdata")]
        public NAAC_AC_434_EContent_DTO editdata([FromBody]NAAC_AC_434_EContent_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _Delobj.editdata(data);
        }

        [Route("deactiveStudent")]
        public NAAC_AC_434_EContent_DTO deactiveStudent([FromBody]NAAC_AC_434_EContent_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _Delobj.deactiveStudent(data);
        }

        [Route("viewuploadflies")]
        public NAAC_AC_434_EContent_DTO viewuploadflies([FromBody] NAAC_AC_434_EContent_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Delobj.viewuploadflies(data);
        }

        [Route("deleteuploadfile")]
        public NAAC_AC_434_EContent_DTO deleteuploadfile([FromBody] NAAC_AC_434_EContent_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Delobj.deleteuploadfile(data);
        }
    }
}
