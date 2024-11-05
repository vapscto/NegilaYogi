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
    public class NonBookTransactionController : Controller
    {
        // GET: api/<controller>

        NonBookTransactionDelegate _delobj = new NonBookTransactionDelegate();

        [Route("getdetails")]
        public NonBookTransaction_DTO getdetails([FromBody] NonBookTransaction_DTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.getdetails(data);
        }

        [Route("studentdetails")]
        public NonBookTransaction_DTO studentdetails([FromBody] NonBookTransaction_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.studentdetails(data);
        }

        [Route("get_staff1")]
        public NonBookTransaction_DTO get_staff1([FromBody] NonBookTransaction_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.get_staff1(data);
        }
        [Route("getdepchange")]
        public NonBookTransaction_DTO getdepchange([FromBody] NonBookTransaction_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.getdepchange(data);
        }

        [Route("get_bookdetails")]
        public NonBookTransaction_DTO get_bookdetails([FromBody] NonBookTransaction_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.get_bookdetails(data);
        }

        [Route("searchfilter")]
        public NonBookTransaction_DTO searchfilter([FromBody] NonBookTransaction_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.searchfilter(data);
        }


        [Route("searchfilterbarcode")]
        public NonBookTransaction_DTO searchfilterbarcode([FromBody] NonBookTransaction_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.searchfilterbarcode(data);
        }

        [Route("searchfilterbarcode1")]
        public NonBookTransaction_DTO searchfilterbarcode1([FromBody] NonBookTransaction_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.searchfilterbarcode1(data);
        }
        [Route("Savedata")]
        public NonBookTransaction_DTO Savedata([FromBody] NonBookTransaction_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return _delobj.Savedata(data);
        }
        [Route("GetStudentDetails1")]
        public NonBookTransaction_DTO GetStudentDetails1([FromBody] NonBookTransaction_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            //    data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.GetStudentDetails1(data);
        }

        [Route("renewaldata")]
        public NonBookTransaction_DTO renewaldata([FromBody] NonBookTransaction_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.renewaldata(data);
        }

        [Route("Editdata")]
        public NonBookTransaction_DTO Editdata([FromBody] NonBookTransaction_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.Editdata(data);
        }

        [Route("returndata")]
        public NonBookTransaction_DTO returndata([FromBody] NonBookTransaction_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.returndata(data);
        }



    }
}
