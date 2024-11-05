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
    public class NaacIPRController : Controller
    {
        NaacIPRDelegate del = new NaacIPRDelegate();

        [Route("loaddata/{id:int}")]
        public NAAC_AC_IPR_322_DTO loaddata(int id)
        {
            NAAC_AC_IPR_322_DTO data = new NAAC_AC_IPR_322_DTO();
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata(data);
        }
        [Route("save")]
        public NAAC_AC_IPR_322_DTO save([FromBody]NAAC_AC_IPR_322_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.save(data);
        }
        [Route("getcomment")]
        public NAAC_AC_IPR_322_DTO getcomment([FromBody]NAAC_AC_IPR_322_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getcomment(data);
        }
        [Route("getfilecomment")]
        public NAAC_AC_IPR_322_DTO getfilecomment([FromBody]NAAC_AC_IPR_322_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getfilecomment(data);
        }
        [Route("savefilewisecomments")]
        public NAAC_AC_IPR_322_DTO savefilewisecomments([FromBody]NAAC_AC_IPR_322_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.savefilewisecomments(data);
        }
        [Route("savemedicaldatawisecomments")]
        public NAAC_AC_IPR_322_DTO savemedicaldatawisecomments([FromBody]NAAC_AC_IPR_322_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.savemedicaldatawisecomments(data);
        }

        [Route("deactive")]
        public NAAC_AC_IPR_322_DTO deactive([FromBody] NAAC_AC_IPR_322_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.deactive(data);
        }
        [Route("EditData")]
        public NAAC_AC_IPR_322_DTO EditData([FromBody]NAAC_AC_IPR_322_DTO category)
        {
            category.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //category.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.EditData(category);
        }



        [Route("viewuploadflies")]
        public NAAC_AC_IPR_322_DTO viewuploadflies([FromBody] NAAC_AC_IPR_322_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.viewuploadflies(data);
        }


        [Route("deleteuploadfile")]
        public NAAC_AC_IPR_322_DTO deleteuploadfile([FromBody] NAAC_AC_IPR_322_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.deleteuploadfile(data);
        }

    }
}
