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
    public class SportStudentHouseDivisionController : Controller
    {
        SportStudentHouseDivisionDelegate mastercastedelStr = new SportStudentHouseDivisionDelegate();


        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails/")]
        public SportMasterHouseDTO Getdetails(SportMasterHouseDTO SportMasterHouseDTO)
        {
            SportMasterHouseDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            SportMasterHouseDTO.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));


            return mastercastedelStr.GetmastercasteData(SportMasterHouseDTO);
        }

        [Route("get_section")]
        public SportMasterHouseDTO get_section([FromBody]SportMasterHouseDTO data)

        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mastercastedelStr.get_section(data);
        }
        [Route("get_student")]
        public SportMasterHouseDTO get_student([FromBody]SportMasterHouseDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mastercastedelStr.get_student(data);
        }
         
        [Route("GetSelectedRowdetails")]
        public SportMasterHouseDTO GetSelectedRowDetails([FromBody]SportMasterHouseDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return mastercastedelStr.GetSelectedRowDetails(data);
        }

        [HttpPost]
        public SportMasterHouseDTO SportMasterHouseDTO([FromBody] SportMasterHouseDTO MMD)
        {
            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mastercastedelStr.mastercasteData(MMD);
        }

        [Route("deactivate")]
        public SportMasterHouseDTO deactivate([FromBody] SportMasterHouseDTO rel)
        {
            return mastercastedelStr.deactivate(rel);
        }
    }
}
