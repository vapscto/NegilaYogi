using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Library;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Library
{
    [Route("api/[controller]")]
    public class CirculationParameterController : Controller
    {
        CirculationParameterDelegate _delobj = new CirculationParameterDelegate();
        [Route("getdetails")]
        public CirculationParameterDTO getdetails([FromBody] CirculationParameterDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.getdetails(data);
        }
        [Route("getdata")]
        public CirculationParameterDTO getdata([FromBody] CirculationParameterDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.getdata(data);
        }

        [Route("gettype")]
        public CirculationParameterDTO gettype([FromBody] CirculationParameterDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.gettype(data);
        }

        [Route("Savedata")]
        public CirculationParameterDTO Savedata([FromBody] CirculationParameterDTO data)
        {
            data.MI_Id= Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.Savedata(data);
        }
        [Route("deactiveY")]
        public CirculationParameterDTO deactiveY([FromBody] CirculationParameterDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.deactiveY(data);
        }
        [Route("editdata")]
        public CirculationParameterDTO editdata([FromBody] CirculationParameterDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.editdata(data);
        }
    }
}
