using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Placement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Placement;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Placement
{
    [Route("api/[controller]")]
    public class PL_CI_StudentStatus : Controller
    {
        PL_CI_StudentStatusDelegate _delg = new PL_CI_StudentStatusDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }



        [Route("loaddata/{id:int}")]
        public PL_CI_StudentStatusDTO loaddata(int id)
        {
            PL_CI_StudentStatusDTO data = new PL_CI_StudentStatusDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.loaddata(data);
        }

       
        [Route("savedetails")]
        public PL_CI_StudentStatusDTO savedetails([FromBody] PL_CI_StudentStatusDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.savedetails(data);
        }


        [Route("editdetails")]
        public PL_CI_StudentStatusDTO editdetails([FromBody] PL_CI_StudentStatusDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.editdetails(data);
        }
        [Route("deactive")]
        public PL_CI_StudentStatusDTO deactive([FromBody] PL_CI_StudentStatusDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.deactive(data);
        }


    }
}
