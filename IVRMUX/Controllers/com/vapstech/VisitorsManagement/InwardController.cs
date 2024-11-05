using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VisitorsManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VisitorsManagement;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.VisitorsManagement
{
    [Route("api/[controller]")]
    public class InwardController : Controller
    {
        InwardDelegate delobj = new InwardDelegate();
        // GET: api/<controller>
       
        [Route("getDetails/{id:int}")]
        public InwardDTO getDetails(int id)
        {
            InwardDTO dto = new InwardDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.getDetails(dto);
        }

        [Route("EditDetails")]
        public InwardDTO EditDetails([FromBody]InwardDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.EditDetails(dto);
        }

        [HttpPost]
        [Route("saveData")]
        public InwardDTO saveData([FromBody]InwardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return delobj.saveData(data);
        }

        [Route("deactivate")]
        public InwardDTO deactivate([FromBody] InwardDTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.deactivate(d);
        }

        [Route("searchfilter")]
        public InwardDTO searchfilter([FromBody] InwardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return delobj.searchfilter(data);
        }

        [Route("get_empdetails")]
        public InwardDTO get_empdetails([FromBody] InwardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.get_empdetails(data);
        }

        [Route("searchfilter2")]
        public InwardDTO searchfilter2([FromBody] InwardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.searchfilter2(data);
        }

        [Route("get_empdetails2")]
        public InwardDTO get_empdetails2([FromBody] InwardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.get_empdetails2(data);
        }

        
    }
}
