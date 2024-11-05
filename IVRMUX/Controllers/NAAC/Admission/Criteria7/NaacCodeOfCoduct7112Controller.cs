using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Admission.Criteria7;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Admission.Criteria7
{
    [Route("api/[controller]")]
    public class NaacCodeOfCoduct7112Controller : Controller
    {
        NaacCodeOfCoduct7112Delegate del = new NaacCodeOfCoduct7112Delegate();
        [Route("loaddata/{id:int}")]
        public NAAC_AC_7112_CodeOfCoduct_DTO loaddata(int id)
        {
            NAAC_AC_7112_CodeOfCoduct_DTO data = new NAAC_AC_7112_CodeOfCoduct_DTO();
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata(data);
        }
        [Route("save")]
        public NAAC_AC_7112_CodeOfCoduct_DTO save([FromBody] NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.save(data);
        }
        [Route("deactivate")]
        public NAAC_AC_7112_CodeOfCoduct_DTO deactivate([FromBody] NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deactivate(data);
        }
        
        [Route("EditData")]
        public NAAC_AC_7112_CodeOfCoduct_DTO EditData([FromBody] NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.EditData(data);
        }
        
        [Route("viewuploadflies")]
        public NAAC_AC_7112_CodeOfCoduct_DTO viewuploadflies([FromBody] NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.viewuploadflies(data);
        }        
        
        [Route("deleteuploadfile")]
        public NAAC_AC_7112_CodeOfCoduct_DTO deleteuploadfile([FromBody] NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deleteuploadfile(data);
        }

        [Route("getcomment")]
        public NAAC_AC_7112_CodeOfCoduct_DTO getcomment([FromBody] NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getcomment(data);
        }

        [Route("getfilecomment")]
        public NAAC_AC_7112_CodeOfCoduct_DTO getfilecomment([FromBody] NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getfilecomment(data);
        }

        [Route("savefilewisecomments")]
        public NAAC_AC_7112_CodeOfCoduct_DTO savefilewisecomments([FromBody] NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savefilewisecomments(data);
        }

        [Route("savemedicaldatawisecomments")]
        public NAAC_AC_7112_CodeOfCoduct_DTO savemedicaldatawisecomments([FromBody] NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savemedicaldatawisecomments(data);
        }

        [Route("getData/{id:int}")]
        public NAAC_AC_7112_CodeOfCoduct_DTO getData(int id)
        {
            NAAC_AC_7112_CodeOfCoduct_DTO data = new NAAC_AC_7112_CodeOfCoduct_DTO();
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getData(data);
        }

    }
}
