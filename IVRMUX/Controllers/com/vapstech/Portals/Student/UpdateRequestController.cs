using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using corewebapi18072016.Delegates.com.vapstech.Portals.Student;
using PreadmissionDTOs.com.vaps.Portals.Student;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using Microsoft.Extensions.Options;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class UpdateRequestController : Controller
    {
        UpdateRequestDelegate fdd = new UpdateRequestDelegate();
        private readonly DomainModelMsSqlServerContext _context;

        public UpdateRequestController(IOptions<FacadeUrl> settings, DomainModelMsSqlServerContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("getloaddata")]
        public UpdateRequestDTO getloaddata(UpdateRequestDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.AMST_ID = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.RoleName = HttpContext.Session.GetString("RoleNme");
          
            return fdd.getloaddata(data);
        }

        [HttpGet]
        [Route("getreploaddata")]
        public UpdateRequestDTO getreploaddata(UpdateRequestDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            
            return fdd.getreploaddata(data);
        }

        [Route("getstudata")]
        public UpdateRequestDTO getstudata([FromBody]UpdateRequestDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            sddto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
          

            return fdd.getstudata(sddto);
        }

        [Route("getreport")]
        public UpdateRequestDTO getreport([FromBody]UpdateRequestDTO sddto)
        {
            sddto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            sddto.AMST_ID = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            sddto.RoleName = HttpContext.Session.GetString("RoleNme");
            return fdd.getreport(sddto);
        }

        [HttpPost]
        [Route("saverequest")]
        public UpdateRequestDTO saverequest([FromBody]UpdateRequestDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.AMST_ID = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            data.IVRMUL_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return fdd.saverequest(data);
        }

        [Route("savedataadmin")]
        public UpdateRequestDTO savedataadmin([FromBody]UpdateRequestDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return fdd.savedataadmin(data);
        }
        [Route("savereject")]
        public UpdateRequestDTO savereject([FromBody]UpdateRequestDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return fdd.savereject(data);
        }

        [Route("searchfilter")]
        public UpdateRequestDTO searchfilter([FromBody]UpdateRequestDTO student)
        {
            student.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            student.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return fdd.searchfilter(student);
        }

        [Route("guardianDetails")]
        public UpdateRequestDTO guardianDetails([FromBody]UpdateRequestDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMST_ID = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return fdd.guardianDetails(data);
        }

        [Route("getdpstate/{id:int}")]
        public UpdateRequestDTO getstate(int id)
        {
            return fdd.getStateByCountry(id);
        }

    }
}
