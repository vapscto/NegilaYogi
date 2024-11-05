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
    public class Staff_BookTranasctionController : Controller
    {
        Staff_BookTranasctionDelegate _delobj = new Staff_BookTranasctionDelegate();

        [Route("getdetails/{id:int}")]
        public Staff_BookTranasctionDTO getdetails(int id)
        {
            Staff_BookTranasctionDTO data = new Staff_BookTranasctionDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delobj.getdetails(data);
        }

        [Route("get_Staffdetails")]
        public Staff_BookTranasctionDTO get_Staffdetails([FromBody] Staff_BookTranasctionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.get_Staffdetails(data);
        }

        [Route("get_bookdetails")]
        public Staff_BookTranasctionDTO get_bookdetails([FromBody] Staff_BookTranasctionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.get_bookdetails(data);
        }

        [Route("Savedata")]
        public Staff_BookTranasctionDTO Savedata([FromBody] Staff_BookTranasctionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.Savedata(data);
        }

        [Route("renewaldata")]
        public Staff_BookTranasctionDTO renewaldata([FromBody] Staff_BookTranasctionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.renewaldata(data);
        }

        [Route("Editdata")]
        public Staff_BookTranasctionDTO Editdata([FromBody] Staff_BookTranasctionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.Editdata(data);
        }

        [Route("returndata")]
        public Staff_BookTranasctionDTO returndata([FromBody] Staff_BookTranasctionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.returndata(data);
        }

    }
}
