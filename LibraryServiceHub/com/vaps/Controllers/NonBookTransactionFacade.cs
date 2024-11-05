using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Library;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class NonBookTransactionFacade : Controller
    {
        // GET: api/<controller>

        public NonBookTransactionInterface _objInter;
        public NonBookTransactionFacade(NonBookTransactionInterface data)
        {
            _objInter = data;
        }

        [Route("getdetails")]
        public NonBookTransaction_DTO getdetails([FromBody]NonBookTransaction_DTO data)
        {
            return _objInter.getdetails(data);
        }
        [Route("searchfilter")]
        public NonBookTransaction_DTO searchfilter([FromBody]NonBookTransaction_DTO data)
        {
            return _objInter.searchfilter(data);
        }
        [Route("searchfilterbarcode")]
        public NonBookTransaction_DTO searchfilterbarcode([FromBody]NonBookTransaction_DTO data)
        {
            return _objInter.searchfilterbarcode(data);
        }
        [Route("searchfilterbarcode1")]
        public NonBookTransaction_DTO searchfilterbarcode1([FromBody]NonBookTransaction_DTO data)
        {
            return _objInter.searchfilterbarcode1(data);
        }

        [Route("studentdetails")]
        public NonBookTransaction_DTO studentdetails([FromBody] NonBookTransaction_DTO data)
        {
            return _objInter.studentdetails(data);
        }
        [Route("get_staff1")]
        public NonBookTransaction_DTO get_staff1([FromBody] NonBookTransaction_DTO data)
        {
            return _objInter.get_staff1(data);
        }
        [Route("getdepchange")]
        public NonBookTransaction_DTO getdepchange([FromBody] NonBookTransaction_DTO data)
        {
            return _objInter.getdepchange(data);
        }

        [Route("get_bookdetails")]
        public NonBookTransaction_DTO get_bookdetails([FromBody] NonBookTransaction_DTO data)
        {
            return _objInter.get_bookdetails(data);
        }

        [Route("Savedata")]
        public NonBookTransaction_DTO Savedata([FromBody] NonBookTransaction_DTO data)
        {
            return _objInter.Savedata(data);
        }
        [Route("GetStudentDetails1")]
        public NonBookTransaction_DTO GetStudentDetails1([FromBody] NonBookTransaction_DTO data)
        {
            return _objInter.GetStudentDetails1(data);
        }
        [Route("renewaldata")]
        public NonBookTransaction_DTO renewaldata([FromBody] NonBookTransaction_DTO data)
        {
            return _objInter.renewaldata(data);
        }

        [Route("Editdata")]
        public NonBookTransaction_DTO Editdata([FromBody] NonBookTransaction_DTO data)
        {
            return _objInter.Editdata(data);
        }

        [Route("returndata")]
        public NonBookTransaction_DTO returndata([FromBody] NonBookTransaction_DTO data)
        {
            return _objInter.returndata(data);
        }
        [Route("getdetails_smartcard")]
        public NonBookTransaction_DTO getdetails_smartcard([FromBody] NonBookTransaction_DTO data)
        {
            return _objInter.getdetails_smartcard(data);
        }




    }
}
