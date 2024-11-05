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
    public class CLGNonBookTransactionController : Controller
    {

        CLGNonBookTransactionDelegate _delobj = new CLGNonBookTransactionDelegate();

        [Route("getdetails")]
        public ClgNonBookTransaction_DTO getdetails([FromBody] ClgNonBookTransaction_DTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));

            return _delobj.getdetails(data);
        }

        [Route("studentdetails")]
        public ClgNonBookTransaction_DTO studentdetails([FromBody] ClgNonBookTransaction_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.studentdetails(data);
        }

        [Route("get_staff1")]
        public ClgNonBookTransaction_DTO get_staff1([FromBody] ClgNonBookTransaction_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.get_staff1(data);
        }
        [Route("getdepchange")]
        public ClgNonBookTransaction_DTO getdepchange([FromBody] ClgNonBookTransaction_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.getdepchange(data);
        }

        [Route("get_bookdetails")]
        public ClgNonBookTransaction_DTO get_bookdetails([FromBody] ClgNonBookTransaction_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.get_bookdetails(data);
        }

        [Route("searchfilter")]
        public ClgNonBookTransaction_DTO searchfilter([FromBody] ClgNonBookTransaction_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.searchfilter(data);
        }


        [Route("searchfilterbarcode")]
        public ClgNonBookTransaction_DTO searchfilterbarcode([FromBody] ClgNonBookTransaction_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.searchfilterbarcode(data);
        }

        [Route("searchfilterbarcode1")]
        public ClgNonBookTransaction_DTO searchfilterbarcode1([FromBody] ClgNonBookTransaction_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.searchfilterbarcode1(data);
        }
        [Route("Savedata")]
        public ClgNonBookTransaction_DTO Savedata([FromBody] ClgNonBookTransaction_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));

            return _delobj.Savedata(data);
        }
        [Route("GetStudentDetails1")]
        public ClgNonBookTransaction_DTO GetStudentDetails1([FromBody] ClgNonBookTransaction_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            //    data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.GetStudentDetails1(data);
        }

        [Route("renewaldata")]
        public ClgNonBookTransaction_DTO renewaldata([FromBody] ClgNonBookTransaction_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.renewaldata(data);
        }

        [Route("Editdata")]
        public ClgNonBookTransaction_DTO Editdata([FromBody] ClgNonBookTransaction_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.Editdata(data);
        }

        [Route("returndata")]
        public ClgNonBookTransaction_DTO returndata([FromBody] ClgNonBookTransaction_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.returndata(data);
        }


    }
}
