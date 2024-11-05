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
    public class NAAC_AC_AwardsController : Controller
    {
        NAAC_AC_AwardsDelegate del = new NAAC_AC_AwardsDelegate();
        [Route("loaddata/{id:int}")]
        public NAAC_AC_Awards_342_DTO loaddata(int id)
        {
            NAAC_AC_Awards_342_DTO data = new NAAC_AC_Awards_342_DTO();
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = id;
            return del.loaddata(data);
        }
        [Route("save")]
        public NAAC_AC_Awards_342_DTO save([FromBody]NAAC_AC_Awards_342_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.save(data);
        }
        [Route("deactive")]
        public NAAC_AC_Awards_342_DTO deactive([FromBody] NAAC_AC_Awards_342_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.deactive(data);
        }
        [Route("EditData")]
        public NAAC_AC_Awards_342_DTO EditData([FromBody] NAAC_AC_Awards_342_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.EditData(data);
        }
        [Route("getfilecomment")]
        public NAAC_AC_Awards_342_DTO getfilecomment([FromBody] NAAC_AC_Awards_342_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getfilecomment(data);
        }
        [Route("savefilewisecomments")]
        public NAAC_AC_Awards_342_DTO savefilewisecomments([FromBody] NAAC_AC_Awards_342_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.savefilewisecomments(data);
        }
        [Route("savemedicaldatawisecomments")]
        public NAAC_AC_Awards_342_DTO savemedicaldatawisecomments([FromBody] NAAC_AC_Awards_342_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.savemedicaldatawisecomments(data);
        }
        [Route("getcomment")]
        public NAAC_AC_Awards_342_DTO getcomment([FromBody] NAAC_AC_Awards_342_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getcomment(data);
        }



        [Route("viewuploadflies")]
        public NAAC_AC_Awards_342_DTO viewuploadflies([FromBody] NAAC_AC_Awards_342_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.viewuploadflies(data);
        }


        [Route("deleteuploadfile")]
        public NAAC_AC_Awards_342_DTO deleteuploadfile([FromBody] NAAC_AC_Awards_342_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.deleteuploadfile(data);
        }



    }
}
