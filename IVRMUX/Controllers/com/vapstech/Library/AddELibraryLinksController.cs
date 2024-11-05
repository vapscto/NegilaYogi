using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.Library;
using PreadmissionDTOs.com.vaps.Library;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Library
{
    [Route("api/[controller]")]
    public class AddELibraryLinksController : Controller
    {
        AddELibraryLinksDelegate _delobj = new AddELibraryLinksDelegate();
        [Route("Savedata")]
        public  AddELibraryLinksDTO Savedata([FromBody] AddELibraryLinksDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.Savedata(data);
        }
        [Route("getdetails/{id:int}")]
        public  AddELibraryLinksDTO getdetails(int id)
        {
             AddELibraryLinksDTO data = new  AddELibraryLinksDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            id = Convert.ToInt32(data.MI_Id);
            return _delobj.getdetails(id);
        }

        [Route("GetELibrary/{id:int}")]
        public  AddELibraryLinksDTO GetELibrary(int id)
        {
             AddELibraryLinksDTO data = new  AddELibraryLinksDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            id = Convert.ToInt32(data.MI_Id);
            return _delobj.GetELibrary(id);
        }
        [Route("deactiveY")]
        public  AddELibraryLinksDTO deactiveY([FromBody] AddELibraryLinksDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.deactiveY(data);
        }
        [Route("geteditdata")]
        public  AddELibraryLinksDTO geteditdata([FromBody] AddELibraryLinksDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.geteditdata(data);
        }
    }
}
