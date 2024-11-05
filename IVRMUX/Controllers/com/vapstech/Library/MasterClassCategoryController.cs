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
    public class MasterClassCategoryController : Controller
    {
        // GET: api/<controller>

        MasterClassCategoryDelegate _delobj = new MasterClassCategoryDelegate();

        [Route("Savedata")]
        public LIB_Master_ClassCategory_DTO Savedata([FromBody]LIB_Master_ClassCategory_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
          
            return _delobj.Savedata(data);
        }
        [Route("getdetails/{id:int}")]
        public LIB_Master_ClassCategory_DTO getdetails(int id)
        {
            LIB_Master_ClassCategory_DTO data = new LIB_Master_ClassCategory_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
          

            return _delobj.getdetails(data);
        }
        [Route("deactiveY")]
        public LIB_Master_ClassCategory_DTO deactiveY([FromBody]LIB_Master_ClassCategory_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.deactiveY(data);
        }

    }
}
