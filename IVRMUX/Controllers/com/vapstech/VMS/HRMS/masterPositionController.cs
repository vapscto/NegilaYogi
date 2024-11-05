using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.VMS.HRMS;
using PreadmissionDTOs.com.vaps.VMS.HRMS;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.VMS.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class masterPositionController : Controller
    {
        masterPositionDelegates del = new masterPositionDelegates();

        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public HR_Master_PositionDTO getalldetails(int id)
        {
            HR_Master_PositionDTO dto = new HR_Master_PositionDTO();
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
        public HR_Master_PositionDTO Post([FromBody]HR_Master_PositionDTO dto)
        {
            //dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRMP_UpdatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savedetails(dto);
        }

        [Route("getdata")]
        public HR_Master_PositionDTO getdata([FromBody] HR_Master_PositionDTO dto)
        {
            //dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRMP_UpdatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getdata(dto);
        }

        [Route("editRecord/{id:int}")]
        public HR_Master_PositionDTO editRecord(int id)
        {
            HR_Master_PositionDTO dto = new HR_Master_PositionDTO();
            //dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRMP_UpdatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getRecorddetailsById(id);
        }

        [Route("ActiveDeactiveRecord/{id:int}")]
        public HR_Master_PositionDTO ActiveDeactiveRecord(int id)
        {
            HR_Master_PositionDTO dto = new HR_Master_PositionDTO();
            dto.HRMP_Id = id;
            //dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.deleterec(dto);
        }
    }
}
