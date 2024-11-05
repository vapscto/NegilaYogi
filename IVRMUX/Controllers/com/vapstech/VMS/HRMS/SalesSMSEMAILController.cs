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
    public class SalesSMSEMAILController : Controller
    {
        SalesSMSEMAILDelegates del = new SalesSMSEMAILDelegates();

        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public SalesSMSEMAILDTO getalldetails(int id)
        {
            SalesSMSEMAILDTO dto = new SalesSMSEMAILDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
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
        [Route("sendsmsemail")]
        public SalesSMSEMAILDTO sendsmsemail([FromBody]SalesSMSEMAILDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRMP_UpdatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.sendsmsemail(dto);
        }

        [Route("editRecord/{id:int}")]
        public SalesSMSEMAILDTO editRecord(int id)
        {
            SalesSMSEMAILDTO dto = new SalesSMSEMAILDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRMP_UpdatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getRecorddetailsById(id);
        }

        [Route("get_state")]
        public SalesSMSEMAILDTO get_state([FromBody] SalesSMSEMAILDTO dto)
        {
            
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.get_state(dto);
        }
        [Route("getrpt")]
        public SalesSMSEMAILDTO getrpt([FromBody] SalesSMSEMAILDTO dto)
        {
            
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getrpt(dto);
        }
        [Route("getrpt_lead")]
        public SalesSMSEMAILDTO getrpt_lead([FromBody] SalesSMSEMAILDTO dto)
        {
            
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getrpt_lead(dto);
        }
        [Route("loadtemplate")]
        public SalesSMSEMAILDTO loadtemplate([FromBody] SalesSMSEMAILDTO dto)
        {
            
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.loadtemplate(dto);
        }

        [Route("viewtemplatedetails")]
        public SalesSMSEMAILDTO viewtemplatedetails([FromBody] SalesSMSEMAILDTO dto)
        {
            
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.viewtemplatedetails(dto);
        }
    }
}
