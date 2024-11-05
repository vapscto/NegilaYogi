using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;
using corewebapi18072016.Delegates.com.vapstech.HRMS;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class MasterIncomeTaxDetailsCessController : Controller
    {
        MasterIncomeTaxDetailsCessDelegate del = new MasterIncomeTaxDetailsCessDelegate();
        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public HR_Master_IncomeTax_Details_CessDTO getalldetails(int id)
        {
            HR_Master_IncomeTax_Details_CessDTO dto = new HR_Master_IncomeTax_Details_CessDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

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
        public HR_Master_IncomeTax_Details_CessDTO Post([FromBody]HR_Master_IncomeTax_Details_CessDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.savedetails(dto);
        }

        [Route("editRecord/{id:int}")]
        public HR_Master_IncomeTax_Details_CessDTO editRecord(int id)
        {
            HR_Master_IncomeTax_Details_CessDTO dto = new HR_Master_IncomeTax_Details_CessDTO();
         
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getRecorddetailsById(id);
        }

        [Route("ActiveDeactiveRecord/{id:int}")]
        public HR_Master_IncomeTax_Details_CessDTO ActiveDeactiveRecord(int id)
        {
            HR_Master_IncomeTax_Details_CessDTO dto = new HR_Master_IncomeTax_Details_CessDTO();
            dto.HRMITDC_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return del.deleterec(dto);
        }
    }
}
