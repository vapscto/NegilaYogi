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
    public class CLGBookTransactionFacade : Controller
    {

        public CLGBookTransactionInterface _objInter;
        public CLGBookTransactionFacade(CLGBookTransactionInterface data)
        {
            _objInter = data;
        }

        [Route("getdetails")]
        public CLGBookTransactionDTO getdetails([FromBody]CLGBookTransactionDTO data)
        {
            return _objInter.getdetails(data);
        }

          [Route("searchfilter")]
        public CLGBookTransactionDTO searchfilter([FromBody]CLGBookTransactionDTO data)
        {
            return _objInter.searchfilter(data);
        }

        [Route("stdSearch_Grid")]
        public CLGBookTransactionDTO stdSearch_Grid([FromBody]CLGBookTransactionDTO data)
        {
            return _objInter.stdSearch_Grid(data);
        }

        [Route("searchfilterbarcode")]
        public CLGBookTransactionDTO searchfilterbarcode([FromBody]CLGBookTransactionDTO data)
        {
            return _objInter.searchfilterbarcode(data);
        }

        [Route("searchfilterbarcode1")]
        public CLGBookTransactionDTO searchfilterbarcode1([FromBody]CLGBookTransactionDTO data)
        {
            return _objInter.searchfilterbarcode1(data);
        }
        
        [Route("studentdetails")]
        public CLGBookTransactionDTO studentdetails([FromBody] CLGBookTransactionDTO data)
        {
            return _objInter.studentdetails(data);
        }

        [Route("get_staff1")]
        public CLGBookTransactionDTO get_staff1([FromBody] CLGBookTransactionDTO data)
        {
            return _objInter.get_staff1(data);
        }

        [Route("getdepchange")]
        public CLGBookTransactionDTO getdepchange([FromBody] CLGBookTransactionDTO data)
        {
            return _objInter.getdepchange(data);
        }

        [Route("get_bookdetails")]
        public CLGBookTransactionDTO get_bookdetails([FromBody] CLGBookTransactionDTO data)
        {
            return _objInter.get_bookdetails(data);
        }

        [Route("Savedata")]
        public CLGBookTransactionDTO Savedata([FromBody] CLGBookTransactionDTO data)
        {
            return _objInter.Savedata(data);
        }

        [Route("GetStudentDetails1")]
        public CLGBookTransactionDTO GetStudentDetails1([FromBody] CLGBookTransactionDTO data)
        {
            return _objInter.GetStudentDetails1(data);
        }

        [Route("renewaldata")]
        public CLGBookTransactionDTO renewaldata([FromBody] CLGBookTransactionDTO data)
        {
            return _objInter.renewaldata(data);
        }

        [Route("Editdata")]
        public CLGBookTransactionDTO Editdata([FromBody] CLGBookTransactionDTO data)
        {
            return _objInter.Editdata(data);
        }

        [Route("returndata")]
        public CLGBookTransactionDTO returndata([FromBody] CLGBookTransactionDTO data)
        {
            return _objInter.returndata(data);
        }
        [Route("showfine")]
        public CLGBookTransactionDTO showfine([FromBody] CLGBookTransactionDTO data)
        {
            return _objInter.showfine(data);
        }

        [Route("getdetails_smartcard")]
        public CLGBookTransactionDTO getdetails_smartcard([FromBody] CLGBookTransactionDTO data)
        {
            return _objInter.getdetails_smartcard(data);
        }
    }
}
