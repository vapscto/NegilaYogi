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
    public class MasterNonBookController : Controller
    {
        // GET: api/<controller>

        MasterNonBookDelegate _delobj = new MasterNonBookDelegate();

        [Route("getdetails/{id:int}")]
        public MatserNonBook_DTO getdetails(int id)
        {
            MatserNonBook_DTO data = new MatserNonBook_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delobj.getdetails(data);
        }

        [Route("Savedata")]
        public MatserNonBook_DTO Savedata([FromBody] MatserNonBook_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return _delobj.Savedata(data);
        }       

        [Route("deactiveY")]
        public MatserNonBook_DTO deactiveY([FromBody] MatserNonBook_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.deactiveY(data);
        }

        [Route("Editdata")]
        public MatserNonBook_DTO Editdata([FromBody] MatserNonBook_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.Editdata(data);
        }

        [Route("searching")]
        public MatserNonBook_DTO searching([FromBody] MatserNonBook_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           
            return _delobj.searching(data);
        }

        [Route("changelibrary")]
        public MatserNonBook_DTO changelibrary([FromBody] MatserNonBook_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           
            return _delobj.changelibrary(data);
        }
        
    }
}
