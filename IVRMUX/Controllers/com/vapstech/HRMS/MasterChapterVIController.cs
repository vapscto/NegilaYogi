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
    public class MasterChapterVIController : Controller
    {
        MasterChapterVIDelegate del = new MasterChapterVIDelegate();

        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public MasterChapterVIDTO getalldetails(int id)
        {
            MasterChapterVIDTO dto = new MasterChapterVIDTO();
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
        public MasterChapterVIDTO Post([FromBody]MasterChapterVIDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.savedetails(dto);
        }

        [Route("editRecord/{id:int}")]
        public MasterChapterVIDTO editRecord(int id)
        {
            MasterChapterVIDTO dto = new MasterChapterVIDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getRecorddetailsById(id);
        }

        [Route("ActiveDeactiveRecord/{id:int}")]
        public MasterChapterVIDTO ActiveDeactiveRecord(int id)
        {
            MasterChapterVIDTO dto = new MasterChapterVIDTO();
            dto.HRMCVIA_Id
 = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.deleterec(dto);
        }


        [HttpPost]
        [Route("validateordernumber")]
        public MasterChapterVIDTO validateordernumber([FromBody] MasterChapterVIDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            categorypage.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.validateordernumber(categorypage);
        }
    }
}
