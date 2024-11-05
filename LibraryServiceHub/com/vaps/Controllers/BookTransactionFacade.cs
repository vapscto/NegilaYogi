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
    public class BookTransactionFacade : Controller
    {

        public BookTransactionInterface _objInter;
        public BookTransactionFacade(BookTransactionInterface data)
        {
            _objInter = data;
        }

        [Route("getdetails")]
        public BookTransactionDTO getdetails([FromBody]BookTransactionDTO data)
        {
            return _objInter.getdetails(data);
        }
        //getdetailsReturn
        [Route("getdetailsReturn")]
        public BookTransactionDTO getdetailsReturn([FromBody]BookTransactionDTO data)
        {
            return _objInter.getdetailsReturn(data);
        }

        [Route("searchfilter")]
        public BookTransactionDTO searchfilter([FromBody]BookTransactionDTO data)
        {
            return _objInter.searchfilter(data);
        }
        [Route("searchfilteravail")]
        public BookTransactionDTO searchfilteravail([FromBody]BookTransactionDTO data)
        {
            return _objInter.searchfilteravail(data);
        }
        [Route("searchfilterbarcode")]
        public BookTransactionDTO searchfilterbarcode([FromBody]BookTransactionDTO data)
        {
            return _objInter.searchfilterbarcode(data);
        }
        [Route("availget_bookdetails")]
        public BookTransactionDTO availget_bookdetails([FromBody]BookTransactionDTO data)
        {
            return _objInter.availget_bookdetails(data);
        }
        [Route("searchfilterbarcode1")]
        public BookTransactionDTO searchfilterbarcode1([FromBody]BookTransactionDTO data)
        {
            return _objInter.searchfilterbarcode1(data);
        }
        [Route("stdSearch_Grid")]
        public BookTransactionDTO stdSearch_Grid([FromBody]BookTransactionDTO data)
        {
            return _objInter.stdSearch_Grid(data);
        }

        [Route("studentdetails")]
        public BookTransactionDTO studentdetails([FromBody] BookTransactionDTO data)
        {
            return _objInter.studentdetails(data);
        }
        [Route("get_staff1")]
        public BookTransactionDTO get_staff1([FromBody] BookTransactionDTO data)
        {
            return _objInter.get_staff1(data);
        }
        [Route("getdepchange")]
        public BookTransactionDTO getdepchange([FromBody] BookTransactionDTO data)
        {
            return _objInter.getdepchange(data);
        }

        [Route("get_bookdetails")]
        public BookTransactionDTO get_bookdetails([FromBody] BookTransactionDTO data)
        {
            return _objInter.get_bookdetails(data);
        }

        [Route("Savedata")]
        public BookTransactionDTO Savedata([FromBody] BookTransactionDTO data)
        {
            return _objInter.Savedata(data);
        }
        [Route("GetStudentDetails1")]
        public BookTransactionDTO GetStudentDetails1([FromBody] BookTransactionDTO data)
        {
            return _objInter.GetStudentDetails1(data);
        }
        [Route("renewaldata")]
        public BookTransactionDTO renewaldata([FromBody] BookTransactionDTO data)
        {
            return _objInter.renewaldata(data);
        }

        [Route("Editdata")]
        public BookTransactionDTO Editdata([FromBody] BookTransactionDTO data)
        {
            return _objInter.Editdata(data);
        }

        [Route("returndata")]
        public BookTransactionDTO returndata([FromBody] BookTransactionDTO data)
        {
            return _objInter.returndata(data);
        }
        [Route("loadbookavail")]
        public BookTransactionDTO loadbookavail([FromBody] BookTransactionDTO data)
        {
            return _objInter.loadbookavail(data);
        }
        [Route("getdetails_smartcard")]
        public BookTransactionDTO getdetails_smartcard([FromBody] BookTransactionDTO data)
        {
            return _objInter.getdetails_smartcard(data);
        }
        [Route("smsdue")]
        public BookTransactionDTO smsdue([FromBody] BookTransactionDTO data)
        {
            return _objInter.smsdue(data);
        }
        [Route("showfine")]
        public BookTransactionDTO showfine([FromBody] BookTransactionDTO data)
        {
            return _objInter.showfine(data);
        }
        [Route("aftersmsdue")]
        public BookTransactionDTO aftersmsdue([FromBody] BookTransactionDTO data)
        {
            return _objInter.aftersmsdue(data);
        }
        //ShowDiffrentDays
        [Route("ShowDiffrentDays")]
        public BookTransactionDTO ShowDiffrentDays([FromBody] BookTransactionDTO data)
        {
            return _objInter.ShowDiffrentDays(data);
        }
        //GettransctionDetails
        [Route("GettransctionDetails")]
        public BookTransactionDTO GettransctionDetails([FromBody] BookTransactionDTO data)
        {
            return _objInter.GettransctionDetails(data);
        }
        [Route("SaveSmartCard")]
        public BookTransactionDTO SaveSmartCard([FromBody] BookTransactionDTO data)
        {
            return _objInter.SaveSmartCard(data);
        }
    }
}
