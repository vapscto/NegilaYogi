using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.Library;
using corewebapi18072016.Delegates.com.vapstech.Library;
using IVRMUX.Delegates.com.vapstech.Library;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Library
{
    [Route("api/[controller]")]
    public class MasterLibraryController : Controller
    {
        MasterLibraryDelegates _delobj = new MasterLibraryDelegates();

       //[HttpPost]
        [Route("Savedata")]
       public LIB_Master_Library_DTO Savedata([FromBody]LIB_Master_Library_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            
            return _delobj.Savedata(data);
        }
        [Route("deactiveY")]
        public LIB_Master_Library_DTO deactiveY([FromBody] LIB_Master_Library_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.deactiveY(data);
        }

        [HttpGet]
        [Route("getdetails/{id:int}")]
        public LIB_Master_Library_DTO getdetails(int id)
        {
            LIB_Master_Library_DTO data = new LIB_Master_Library_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.getdetails(data);
        }

        [Route("saveclassdata")]
        public LIB_Master_Library_DTO saveclassdata([FromBody]LIB_Master_Library_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _delobj.saveclassdata(data);
        }
        [Route("deactiveYstf")]
        public LIB_Master_Library_DTO deactiveYstf([FromBody] LIB_Master_Library_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.deactiveYstf(data);
        }

        [Route("EditstaffData")]
        public LIB_Master_Library_DTO EditstaffData([FromBody] LIB_Master_Library_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.EditstaffData(data);
        }

        [Route("modalclsslst")]
        public LIB_Master_Library_DTO modalclsslst([FromBody] LIB_Master_Library_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.modalclsslst(data);
        }
        [Route("deactivclsdata")]
        public LIB_Master_Library_DTO deactivclsdata([FromBody] LIB_Master_Library_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.deactivclsdata(data);
        }

        [Route("getusername")]
        public LIB_Master_Library_DTO getusername([FromBody] LIB_Master_Library_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.getusername(data);
        }
        [Route("check_userclass")]
        public LIB_Master_Library_DTO check_userclass([FromBody] LIB_Master_Library_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.check_userclass(data);
        }

        [Route("EditclassData")]
        public LIB_Master_Library_DTO EditclassData([FromBody] LIB_Master_Library_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.EditclassData(data);
        }

        [Route("get_MappedClasslist")]
        public LIB_Master_Library_DTO get_MappedClasslist([FromBody] LIB_Master_Library_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.get_MappedClasslist(data);
        }


        [Route("savestaffdata")]
        public LIB_Master_Library_DTO savestaffdata([FromBody] LIB_Master_Library_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.savestaffdata(data);
        }

        
    }
}
