
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model.com.vaps.Exam;
using PreadmissionDTOs.com.vaps.Exam;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [Route("api/[controller]")]
    public class HHSReport_5to7Controller : Controller
    {
        HHSReport_5to7Delegates crStr = new HHSReport_5to7Delegates();
      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
      
        [HttpGet]
        [Route("Getdetails")]
        public HHSReport_5to7DTO Getdetails(HHSReport_5to7DTO data)
         {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.Getdetails(data);
        }

        [HttpPost]
        [Route("savedetails")]
        public HHSReport_5to7DTO savedetails([FromBody] HHSReport_5to7DTO data)
        {           
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.savedetails(data);
        }

        [Route("getclass/{id}")]
        public HHSReport_5to7DTO getclass(int id)
        {
            HHSReport_5to7DTO data = new HHSReport_5to7DTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = id;            
            return crStr.getclass(data);
        }

        [HttpPost]
        [Route("Getsection")]
        public HHSReport_5to7DTO Getsection([FromBody] HHSReport_5to7DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.Getsection(data);
        }

        [HttpPost]
        [Route("GetAttendence")]
        public HHSReport_5to7DTO GetAttendence([FromBody] HHSReport_5to7DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));            
            return crStr.GetAttendence(data);
        }
        
        [Route("Get_primary_savedetails")]
        public HHSReport_5to7DTO Get_primary_savedetails([FromBody] HHSReport_5to7DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));            
            return crStr.Get_primary_savedetails(data);
        }
    }
}