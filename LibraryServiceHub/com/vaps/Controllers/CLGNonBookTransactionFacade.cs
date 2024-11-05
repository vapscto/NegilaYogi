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
    public class CLGNonBookTransactionFacade : Controller
    {
        public CLGNonBookTransactionInterface _objInter;
        public CLGNonBookTransactionFacade(CLGNonBookTransactionInterface data)
        {
            _objInter = data;
        }

        [Route("getdetails")]
        public ClgNonBookTransaction_DTO getdetails([FromBody]ClgNonBookTransaction_DTO data)
        {
            return _objInter.getdetails(data);
        }
        [Route("searchfilter")]
        public ClgNonBookTransaction_DTO searchfilter([FromBody]ClgNonBookTransaction_DTO data)
        {
            return _objInter.searchfilter(data);
        }
        [Route("searchfilterbarcode")]
        public ClgNonBookTransaction_DTO searchfilterbarcode([FromBody]ClgNonBookTransaction_DTO data)
        {
            return _objInter.searchfilterbarcode(data);
        }
        [Route("searchfilterbarcode1")]
        public ClgNonBookTransaction_DTO searchfilterbarcode1([FromBody]ClgNonBookTransaction_DTO data)
        {
            return _objInter.searchfilterbarcode1(data);
        }

        [Route("studentdetails")]
        public ClgNonBookTransaction_DTO studentdetails([FromBody] ClgNonBookTransaction_DTO data)
        {
            return _objInter.studentdetails(data);
        }
        [Route("get_staff1")]
        public ClgNonBookTransaction_DTO get_staff1([FromBody] ClgNonBookTransaction_DTO data)
        {
            return _objInter.get_staff1(data);
        }
        [Route("getdepchange")]
        public ClgNonBookTransaction_DTO getdepchange([FromBody] ClgNonBookTransaction_DTO data)
        {
            return _objInter.getdepchange(data);
        }

        [Route("get_bookdetails")]
        public ClgNonBookTransaction_DTO get_bookdetails([FromBody] ClgNonBookTransaction_DTO data)
        {
            return _objInter.get_bookdetails(data);
        }

        [Route("Savedata")]
        public ClgNonBookTransaction_DTO Savedata([FromBody] ClgNonBookTransaction_DTO data)
        {
            return _objInter.Savedata(data);
        }
        [Route("GetStudentDetails1")]
        public ClgNonBookTransaction_DTO GetStudentDetails1([FromBody] ClgNonBookTransaction_DTO data)
        {
            return _objInter.GetStudentDetails1(data);
        }
        [Route("renewaldata")]
        public ClgNonBookTransaction_DTO renewaldata([FromBody] ClgNonBookTransaction_DTO data)
        {
            return _objInter.renewaldata(data);
        }

        [Route("Editdata")]
        public ClgNonBookTransaction_DTO Editdata([FromBody] ClgNonBookTransaction_DTO data)
        {
            return _objInter.Editdata(data);
        }

        [Route("returndata")]
        public ClgNonBookTransaction_DTO returndata([FromBody] ClgNonBookTransaction_DTO data)
        {
            return _objInter.returndata(data);
        }
        [Route("getdetails_smartcard")]
        public ClgNonBookTransaction_DTO getdetails_smartcard([FromBody] ClgNonBookTransaction_DTO data)
        {
            return _objInter.getdetails_smartcard(data);
        }

    }
}
