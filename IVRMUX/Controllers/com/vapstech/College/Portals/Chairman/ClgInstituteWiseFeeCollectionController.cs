using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using corewebapi18072016.Delegates;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Portals.Chairman;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [Route("api/[controller]")]
    public class ClgInstituteWiseFeeCollectionController : Controller
    {
        ClgInstituteWiseFeeCollectionDelegate crStr = new ClgInstituteWiseFeeCollectionDelegate();

        [HttpGet]
        [Route("Getdetails")]
        public ClgInstituteWiseFeeCollectionDTO Getdetails(ClgInstituteWiseFeeCollectionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return crStr.Getdetails(data);
        }


        [Route("getdata/{id}")]
        public ClgInstituteWiseFeeCollectionDTO getdata(int id)
        {
            ClgInstituteWiseFeeCollectionDTO data = new ClgInstituteWiseFeeCollectionDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = id;
            return crStr.Getdetails(data);
        }

        [HttpPost]
        [Route("Getsectioncount")]
        public ClgInstituteWiseFeeCollectionDTO Getsectioncount([FromBody] ClgInstituteWiseFeeCollectionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            // data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return crStr.Getsectioncount(data);
        }

    }
}
