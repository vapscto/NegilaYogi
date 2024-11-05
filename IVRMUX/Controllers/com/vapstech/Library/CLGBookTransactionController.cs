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
    public class CLGBookTransactionController : Controller
    {
        CLGBookTransactionDelegate _delobj = new CLGBookTransactionDelegate();

        [Route("getdetails")]
        public CLGBookTransactionDTO getdetails([FromBody] CLGBookTransactionDTO data)
        {
           
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.getdetails(data);
        }

        [Route("studentdetails")]  
        public CLGBookTransactionDTO studentdetails([FromBody] CLGBookTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.studentdetails(data);
        }

        [Route("get_staff1")]  
        public CLGBookTransactionDTO get_staff1([FromBody] CLGBookTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.get_staff1(data);
        }
        [Route("getdepchange")]  
        public CLGBookTransactionDTO getdepchange([FromBody] CLGBookTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.getdepchange(data);
        }

        [Route("get_bookdetails")] 
        public CLGBookTransactionDTO get_bookdetails([FromBody] CLGBookTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.get_bookdetails(data);
        }

        [Route("searchfilter")] 
        public CLGBookTransactionDTO searchfilter([FromBody] CLGBookTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.searchfilter(data);
        }
        [Route("stdSearch_Grid")] 
        public CLGBookTransactionDTO stdSearch_Grid([FromBody] CLGBookTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.stdSearch_Grid(data);
        }


        [Route("searchfilterbarcode")]
        public CLGBookTransactionDTO searchfilterbarcode([FromBody] CLGBookTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.searchfilterbarcode(data);
        }

        [Route("searchfilterbarcode1")]
        public CLGBookTransactionDTO searchfilterbarcode1([FromBody] CLGBookTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.searchfilterbarcode1(data);
        }
        [Route("Savedata")]  
        public CLGBookTransactionDTO Savedata([FromBody] CLGBookTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.Savedata(data);
        }
        [Route("GetStudentDetails1")]  
        public CLGBookTransactionDTO GetStudentDetails1([FromBody] CLGBookTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

        //    data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.GetStudentDetails1(data);
        }

        [Route("renewaldata")]  
        public CLGBookTransactionDTO renewaldata([FromBody] CLGBookTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.renewaldata(data);
        }

        [Route("Editdata")]
        public CLGBookTransactionDTO Editdata([FromBody] CLGBookTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.Editdata(data);
        }

        [Route("returndata")]
        public CLGBookTransactionDTO returndata([FromBody] CLGBookTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.returndata(data);
        }

        [Route("showfine")]
        public CLGBookTransactionDTO showfine([FromBody] CLGBookTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.showfine(data);
        }
    }
}
