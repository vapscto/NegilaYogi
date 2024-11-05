using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Sports;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Sports
{
    [Route("api/[controller]")]
    public class SportMasterHouseCommitteController : Controller
    {
        SportMasterHouseCommitteDelegate mastercastedelStr = new SportMasterHouseCommitteDelegate();

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails")]
        public HouseCommitte_DTO Getdetails(HouseCommitte_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return mastercastedelStr.GetmastercasteData(data);
        }

        [Route("get_section")]
        public HouseCommitte_DTO get_section([FromBody]HouseCommitte_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mastercastedelStr.get_section(data);
        }
        [Route("get_student")]
        public HouseCommitte_DTO get_student([FromBody]HouseCommitte_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mastercastedelStr.get_student(data);
        }

        [Route("GetSelectedRowdetails")]
        public HouseCommitte_DTO GetSelectedRowDetails([FromBody]HouseCommitte_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return mastercastedelStr.GetSelectedRowDetails(data);
        }

        [HttpPost]
        public HouseCommitte_DTO HouseCommitte_DTO([FromBody] HouseCommitte_DTO MMD)
        {
            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return mastercastedelStr.mastercasteData(MMD);
        }

        [Route("deactivate")]
        public HouseCommitte_DTO deactivate([FromBody] HouseCommitte_DTO rel)
        {
            rel.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return mastercastedelStr.deactivate(rel);
        }

        [Route("onhousechage")]
        public HouseCommitte_DTO onhousechage([FromBody] HouseCommitte_DTO rel)
        {
            rel.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           
            return mastercastedelStr.onhousechage(rel); 
        }

        [Route("get_House")]
        public HouseCommitte_DTO get_House([FromBody] HouseCommitte_DTO rel)
        {
            rel.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
          
            return mastercastedelStr.get_House(rel);
        }

        
    }
}
