using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.University;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.Medical;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.University
{
    [Route("api/[controller]")]
    public class HSU_MasterCR2Controller : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public HSU_MasterCR2Delegate _objdel = new HSU_MasterCR2Delegate();


        [Route("loaddata/{id:int}")]
        public HSU_MasterCR2_DTO loaddata(int id)
        {
            HSU_MasterCR2_DTO data = new HSU_MasterCR2_DTO();
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.loaddata(data);
        }

        [Route("save_HSU_221")]
        public HSU_MasterCR2_DTO save_HSU_221([FromBody]HSU_MasterCR2_DTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.save_HSU_221(data);
        }

        [Route("save_HSU_232")]
        public HSU_MasterCR2_DTO save_HSU_232([FromBody]HSU_MasterCR2_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.save_HSU_232(data);
        }

        [Route("save_HSU_255")]
        public HSU_MasterCR2_DTO save_HSU_255([FromBody]HSU_MasterCR2_DTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.save_HSU_255(data);
        }
    }
}
