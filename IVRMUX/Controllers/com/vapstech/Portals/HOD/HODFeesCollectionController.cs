using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Portals.HOD;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Portals.HOD
{
    [Route("api/[controller]")]
    public class HODFeesCollectionController : Controller
    {
        HODFeesCollectionDelegates crStr = new HODFeesCollectionDelegates();
        // GET: /<controller>/
      
      
        [Route("Getdetails")]
        public FEESOverAllStatusSchoolDTO Getdetails()
        {
            FEESOverAllStatusSchoolDTO data = new FEESOverAllStatusSchoolDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return crStr.Getdetails(data);
        }


        [Route("getdata/{id}")]
        public FEESOverAllStatusSchoolDTO getdata(int id)
        {
            FEESOverAllStatusSchoolDTO data = new FEESOverAllStatusSchoolDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = id;
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return crStr.Getdetails(data);
        }

        
        [Route("Getsectioncount")]
        public FEESOverAllStatusSchoolDTO Getsectioncount([FromBody] FEESOverAllStatusSchoolDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return crStr.Getsectioncount(data);
        }
    }
}
