using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Library.Reports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Library;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Library.Reports
{
    [Route("api/[controller]")]
    public class BookStatusDetailsController : Controller
    {
        BookStatusDetailsDelegate _delobj = new BookStatusDetailsDelegate();

        [Route("getdetails/{id:int}")]
        public BookStatusDetailsDTO getdetails(int id)
        {
            BookStatusDetailsDTO data = new BookStatusDetailsDTO();

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));         
            data.IVRMUL_Id= Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
         
            return _delobj.getdetails(data);
        }

        [Route("get_report")]
        public BookStatusDetailsDTO get_report([FromBody] BookStatusDetailsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delobj.get_report(data);
        }
        [Route("searchfilter")]
        public BookStatusDetailsDTO searchfilter([FromBody] BookStatusDetailsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delobj.searchfilter(data);
        }
        [Route("get_bookdetails")]
        public BookStatusDetailsDTO get_bookdetails([FromBody] BookStatusDetailsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delobj.get_bookdetails(data);
        }
    }
}
