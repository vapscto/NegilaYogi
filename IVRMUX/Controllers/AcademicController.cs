using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class AcademicController : Controller
    {
        AcademicDelegate ad = new AcademicDelegate();

        // GET: api/Academic/5
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public AcademicDTO Get([FromQuery] int id)
        {
            AcademicDTO dto = new AcademicDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId  = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            int roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.roleId = roleidd;

            return ad.getAcademicdata(dto);
        }

        // POST: api/Academic
        [HttpPost]
        public AcademicDTO Post([FromBody]AcademicDTO ac)
        {
            int roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            ac.roleId = roleidd;

            ac.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ad.savedetails(ac);
        }
        [Route("getdetails/{id:int}")]
        public AcademicDTO getdetail(int id)
        {
            return ad.editAcademic(id);
        }
      
        [Route("deletedetails/{id:int}")]
        public AcademicDTO Delete(int id)
        {
            AcademicDTO dto = new AcademicDTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = id;
            int roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.roleId = roleidd;
            dto.MI_Id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.deleterec(dto);
        }
       
        [HttpPost]
        [Route("deactivate")]
        public AcademicDTO deactvate([FromBody] AcademicDTO dto)
        {
            int roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.roleId = roleidd;
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = dto.ASMAY_Id;
            return ad.deactivateAcademicYear(dto);
        }
        [Route("SearchByColumn")]
        public AcademicDTO searchByColumn([FromBody] AcademicDTO data)
        {
            int roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.roleId = roleidd;
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ad.searchByColumn(data);
        }

        [Route("saveorder")]
        public AcademicDTO saveorder([FromBody] AcademicDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ad.saveorder(data);
        }

    }
}
