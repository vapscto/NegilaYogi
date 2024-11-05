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
    public class HumanValuesController : Controller
    {
        public HumanValuesDelegate _objdel = new HumanValuesDelegate();

        [Route("loaddata/{id:int}")]
        public NAAC_AC_7114_HumanValues_DTO loaddata(int id)
        {
            NAAC_AC_7114_HumanValues_DTO data = new NAAC_AC_7114_HumanValues_DTO();
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.loaddata(data);
        }

        [Route("save")]
        public NAAC_AC_7114_HumanValues_DTO savedatatab1([FromBody] NAAC_AC_7114_HumanValues_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savedatatab1(data);
        }

        [Route("EditData")]
        public NAAC_AC_7114_HumanValues_DTO editTab1([FromBody] NAAC_AC_7114_HumanValues_DTO data)
        {
            return _objdel.editTab1(data);
        }

        [Route("deactivate")]
        public NAAC_AC_7114_HumanValues_DTO deactivYTab1([FromBody] NAAC_AC_7114_HumanValues_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.deactivYTab1(data);
        }

        [Route("deleteuploadfile")]
        public NAAC_AC_7114_HumanValues_DTO deleteuploadfile([FromBody] NAAC_AC_7114_HumanValues_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.deleteuploadfile(data);
        }
         [Route("getcomment")]
        public NAAC_AC_7114_HumanValues_DTO getcomment([FromBody] NAAC_AC_7114_HumanValues_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getcomment(data);
        }
        [Route("getfilecomment")]
        public NAAC_AC_7114_HumanValues_DTO getfilecomment([FromBody] NAAC_AC_7114_HumanValues_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getfilecomment(data);
        }
        [Route("savecomments")]
        public NAAC_AC_7114_HumanValues_DTO savecomments([FromBody] NAAC_AC_7114_HumanValues_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savecomments(data);
        }
        [Route("savefilewisecomments")]
        public NAAC_AC_7114_HumanValues_DTO savefilewisecomments([FromBody] NAAC_AC_7114_HumanValues_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savefilewisecomments(data);
        }

        [Route("getData/{id:int}")]
        public NAAC_AC_7114_HumanValues_DTO getData(int id)
        {
            NAAC_AC_7114_HumanValues_DTO data = new NAAC_AC_7114_HumanValues_DTO();
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getData(data);
        }
    }
}
