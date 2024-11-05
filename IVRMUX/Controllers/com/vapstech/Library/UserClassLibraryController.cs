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
    public class UserClassLibraryController : Controller
    {

        UserClassLibraryDelegate _delobj = new UserClassLibraryDelegate();
        // GET: api/<controller>

        [Route("getdetails/{id:int}")]
        public LIB_Library_Class_DTO getdetails(int id)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            id = mi_id;
            return _delobj.getdetails(id);
        }

    }
}
