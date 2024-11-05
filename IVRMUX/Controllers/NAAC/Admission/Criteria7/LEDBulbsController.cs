using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Admission.Criteria7
{
    [Route("api/[controller]")]
    public class LEDBulbsController : Controller
    {
        public LEDBulbsDelegate _objdel = new LEDBulbsDelegate();

        [Route("loaddata/{id:int}")]
        public NAAC_AC_714_LEDBulbs_DTO loaddata(int id)
        {
            NAAC_AC_714_LEDBulbs_DTO data = new NAAC_AC_714_LEDBulbs_DTO();
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.loaddata(data);
        }
        [Route("save")]
        public NAAC_AC_714_LEDBulbs_DTO savedatatab1([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savedatatab1(data);
        }
        [Route("getfilecommentLEDbulb")]
        public NAAC_AC_714_LEDBulbs_DTO getfilecommentLEDbulb([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getfilecommentLEDbulb(data);
        }
        [Route("getcommentLEDbulb")]
        public NAAC_AC_714_LEDBulbs_DTO getcommentLEDbulb([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getcommentLEDbulb(data);
        }
        [Route("savefilewisecommentsLEDbulb")]
        public NAAC_AC_714_LEDBulbs_DTO savefilewisecommentsLEDbulb([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savefilewisecommentsLEDbulb(data);
        }
        [Route("savemedicaldatawisecommentsLEDbulb")]
        public NAAC_AC_714_LEDBulbs_DTO savemedicaldatawisecommentsLEDbulb([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savemedicaldatawisecommentsLEDbulb(data);
        }

        [Route("EditData")]
        public NAAC_AC_714_LEDBulbs_DTO editTab1([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            return _objdel.editTab1(data);
        }

        [Route("deactivate")]
        public NAAC_AC_714_LEDBulbs_DTO deactivYTab1([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.deactivYTab1(data);
        }

        [Route("deleteuploadfile")]
        public NAAC_AC_714_LEDBulbs_DTO deleteuploadfile([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.deleteuploadfile(data);
        }

        [Route("getData/{id:int}")]
        public NAAC_AC_714_LEDBulbs_DTO getData(int id)
        {
            NAAC_AC_714_LEDBulbs_DTO data = new NAAC_AC_714_LEDBulbs_DTO();
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getData(data);
        }

        //MC
        [Route("getDataMCwater/{id:int}")]
        public NAAC_AC_714_LEDBulbs_DTO getDataMCwater(int id)
        {
            NAAC_AC_714_LEDBulbs_DTO data = new NAAC_AC_714_LEDBulbs_DTO();
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getDataMCwater(data);
        }
        [Route("saveMCwater")]
        public NAAC_AC_714_LEDBulbs_DTO saveMCwater([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.saveMCwater(data);
        }
        [Route("getcomment")]
        public NAAC_AC_714_LEDBulbs_DTO getcomment([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getcomment(data);
        }
        
       
        
        [Route("savemedicaldatawisecomments")]
        public NAAC_AC_714_LEDBulbs_DTO savemedicaldatawisecomments([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savemedicaldatawisecomments(data);
        }

        [Route("EditDataMCwater")]
        public NAAC_AC_714_LEDBulbs_DTO EditDataMCwater([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            return _objdel.EditDataMCwater(data);
        }

        [Route("deactivateMCwater")]
        public NAAC_AC_714_LEDBulbs_DTO deactivateMCwater([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.deactivateMCwater(data);
        }

        [Route("getDataMCgreen/{id:int}")]
        public NAAC_AC_714_LEDBulbs_DTO getDataMCgreen(int id)
        {
            NAAC_AC_714_LEDBulbs_DTO data = new NAAC_AC_714_LEDBulbs_DTO();
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getDataMCgreen(data);
        }
        [Route("saveMCgreen")]
        public NAAC_AC_714_LEDBulbs_DTO saveMCgreen([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.saveMCgreen(data);
        }

        [Route("EditDataMCgreen")]
        public NAAC_AC_714_LEDBulbs_DTO EditDataMCgreen([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            return _objdel.EditDataMCgreen(data);
        }

        [Route("deactivateMCgreen")]
        public NAAC_AC_714_LEDBulbs_DTO deactivateMCgreen([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.deactivateMCgreen(data);
        }

        [Route("getDataMCdisable/{id:int}")]
        public NAAC_AC_714_LEDBulbs_DTO getDataMCdisable(int id)
        {
            NAAC_AC_714_LEDBulbs_DTO data = new NAAC_AC_714_LEDBulbs_DTO();
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getDataMCdisable(data);
        }
        [Route("saveMCdisable")]
        public NAAC_AC_714_LEDBulbs_DTO saveMCdisable([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.saveMCdisable(data);
        }

        [Route("EditDataMCdisable")]
        public NAAC_AC_714_LEDBulbs_DTO EditDataMCdisable([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            return _objdel.EditDataMCdisable(data);
        }

        [Route("deactivateMCdisable")]
        public NAAC_AC_714_LEDBulbs_DTO deactivateMCdisable([FromBody] NAAC_AC_714_LEDBulbs_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.deactivateMCdisable(data);
        }
        //MC
    }
}
