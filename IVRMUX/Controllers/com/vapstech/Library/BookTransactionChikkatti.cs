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
    public class BookTransactionChikkatti : Controller
    {
        BookTransactionChikkattiDelegate _delobj = new BookTransactionChikkattiDelegate();

        [Route("getdetails")]
        public BookTransactionDTO getdetails([FromBody] BookTransactionDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.getdetails(data);
        }

        [Route("studentdetails")]
        public BookTransactionDTO studentdetails([FromBody] BookTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.studentdetails(data);
        }

        [Route("get_staff1")]
        public BookTransactionDTO get_staff1([FromBody] BookTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.get_staff1(data);
        }
        [Route("getdepchange")]
        public BookTransactionDTO getdepchange([FromBody] BookTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.getdepchange(data);
        }

        [Route("get_bookdetails")]
        public BookTransactionDTO get_bookdetails([FromBody] BookTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.get_bookdetails(data);
        }

        [Route("searchfilter")]
        public BookTransactionDTO searchfilter([FromBody] BookTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.searchfilter(data);
        }

        [Route("searchfilteravail")]
        public BookTransactionDTO searchfilteravail([FromBody] BookTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.searchfilteravail(data);
        }
        [Route("availget_bookdetails")]
        public BookTransactionDTO availget_bookdetails([FromBody] BookTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.availget_bookdetails(data);
        }


        [Route("searchfilterbarcode")]
        public BookTransactionDTO searchfilterbarcode([FromBody] BookTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.searchfilterbarcode(data);
        }
        [Route("stdSearch_Grid")]
        public BookTransactionDTO stdSearch_Grid([FromBody] BookTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.stdSearch_Grid(data);
        }

        [Route("searchfilterbarcode1")]
        public BookTransactionDTO searchfilterbarcode1([FromBody] BookTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.IVRMUL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delobj.searchfilterbarcode1(data);
        }
        [Route("Savedata")]
        public BookTransactionDTO Savedata([FromBody] BookTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.Savedata(data);
        }
        [Route("GetStudentDetails1")]
        public BookTransactionDTO GetStudentDetails1([FromBody] BookTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            //    data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.GetStudentDetails1(data);
        }

        [Route("renewaldata")]
        public BookTransactionDTO renewaldata([FromBody] BookTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.renewaldata(data);
        }

        [Route("Editdata")]
        public BookTransactionDTO Editdata([FromBody] BookTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.Editdata(data);
        }

        [Route("returndata")]
        public BookTransactionDTO returndata([FromBody] BookTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.returndata(data);
        }
        [Route("loadbookavail")]
        public BookTransactionDTO loadbookavail([FromBody] BookTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.loadbookavail(data);
        }
        [Route("showfine")]
        public BookTransactionDTO showfine([FromBody] BookTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.showfine(data);
        }

    }
}
