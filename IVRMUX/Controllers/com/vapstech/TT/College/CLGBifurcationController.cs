using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs.com.vaps.TT;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.TT.College;


namespace IVRMUX.Controllers.com.vapstech.TT.College
{
    [Route("api/[controller]")]
    public class CLGBifurcationController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        CLGBifurcationDelegate ad = new CLGBifurcationDelegate();

        // GET: api/Academic/5
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public CLGBifurcationDTO Get([FromQuery] int id)
        {
            CLGBifurcationDTO data = new CLGBifurcationDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.getalldetails(data);
        }
        [HttpGet]
        [Route("editDay/{id:int}")]
        public CLGBifurcationDTO editDay(int id)
        {
            CLGBifurcationDTO data = new CLGBifurcationDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.TTMD_Id = id;
            return ad.editDay(data);
        }

        // POST: api/Academic
        [HttpPost]
        [Route("getBranch")]
        public CLGBifurcationDTO getBranch([FromBody] CLGBifurcationDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.getBranch(data);
        }
        [HttpPost]
        [Route("savedetail")]
        public CLGBifurcationDTO savedetailBiff([FromBody] CLGBifurcationDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.savedetailBiff(data);
        }

        [HttpPost]
        [Route("editbiff")]
        public CLGBifurcationDTO editbiff([FromBody] CLGBifurcationDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.editbiff(data);
        }

        [HttpPost]
        [Route("deactivatebiff")]
        public CLGBifurcationDTO deactivatebiff([FromBody] CLGBifurcationDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.deactivatebiff(data);
        }
      
         [HttpPost]
        [Route("viewrecordspopup")]
        public CLGBifurcationDTO viewrecordspopup([FromBody] CLGBifurcationDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.viewrecordspopup(data);
        }
      

      


    }
}
