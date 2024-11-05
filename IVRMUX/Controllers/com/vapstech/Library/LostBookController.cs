using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Library;
using IVRMUX.Delegates.com.vapstech.Library.Reports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Library;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Library
{
    [Route("api/[controller]")]
    public class LostBookController : Controller
    {

        LostBookDelegate _delobj = new LostBookDelegate();

        [Route("getdetails")]
        public LostBook_DTO getdetails([FromBody] LostBook_DTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delobj.getdetails(data);
        }

        [Route("searchfilter")]
        public LostBook_DTO searchfilter([FromBody] LostBook_DTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        
            return _delobj.searchfilter(data);
        }

        [Route("get_authorNm")]
        public LostBook_DTO get_authorNm([FromBody] LostBook_DTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           
            return _delobj.get_authorNm(data);
        }

        [Route("get_radiochange")]
        public LostBook_DTO get_radiochange([FromBody] LostBook_DTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
          
            return _delobj.get_radiochange(data);
        }

        [Route("saverecord")]
        public LostBook_DTO saverecord([FromBody] LostBook_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Login_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.saverecord(data);
        }

    }
}
