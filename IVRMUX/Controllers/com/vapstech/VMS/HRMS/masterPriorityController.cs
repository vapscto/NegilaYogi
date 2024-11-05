using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.VMS.HRMS;
using PreadmissionDTOs.com.vaps.VMS.HRMS;


namespace corewebapi18072016.Controllers.com.vapstech.VMS.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class masterPriorityController : Controller
    {
        masterPriorityDelegates del = new masterPriorityDelegates();

        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public HR_Master_PriorityDTO getalldetails(int id)
        {
            HR_Master_PriorityDTO dto = new HR_Master_PriorityDTO();
            //dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.onloadgetdetails(dto);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public HR_Master_PriorityDTO Post([FromBody]HR_Master_PriorityDTO dto)
        {
            //dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRMP_UpdatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savedetails(dto);
        }
        [Route("getdata")]
        public HR_Master_PriorityDTO getdata([FromBody] HR_Master_PriorityDTO dto)
        {
            //dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRMP_UpdatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getdata(dto);
        }

        [Route("editRecord/{id:int}")]
        public HR_Master_PriorityDTO editRecord(int id)
        {
            HR_Master_PriorityDTO dto = new HR_Master_PriorityDTO();
            //dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRMP_UpdatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getRecorddetailsById(id);
        }

        [Route("ActiveDeactiveRecord/{id:int}")]
        public HR_Master_PriorityDTO ActiveDeactiveRecord(int id)
        {
            HR_Master_PriorityDTO dto = new HR_Master_PriorityDTO();
            dto.HRMPR_Id = id;
            //dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.deleterec(dto);
        }
    }
}
