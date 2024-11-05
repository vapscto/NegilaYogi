using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using PreadmissionDTOs.com.vaps.Fees;
using System.IO;
using System.Net;
using System.Text;
using MimeKit;
using MailKit.Net.Smtp;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeStudentTransactionFacade : Controller
    {
        public FeeStudentTransactionInterface _org;

        public FeeStudentTransactionFacade(FeeStudentTransactionInterface orga)
        {
            _org = orga;
        }

        [HttpPost]
        [Route("getalldetails")]
        public FeeStudentTransactionDTO Getdet([FromBody] FeeStudentTransactionDTO data)
        {
            return _org.getdata(data);
        }


        [Route("getstudlistgroup")]
        public FeeStudentTransactionDTO Getstudacademicgrp([FromBody] FeeStudentTransactionDTO data)
        {
            return _org.getdatastuacadgrp(data);
        }

        [Route("printreceipt")]
        public Task<FeeStudentTransactionDTO> printreceipt([FromBody] FeeStudentTransactionDTO data)
        {
            return _org.printreceipt(data);
        }


        [HttpPost]
        public Task<FeeStudentTransactionDTO> savedata([FromBody] FeeStudentTransactionDTO pgmodu)
        {
            return _org.savedetails(pgmodu);
        }

        [Route("getacademicyear")]
        public FeeStudentTransactionDTO Getstudacademic([FromBody] FeeStudentTransactionDTO data)
        {
            return _org.getdatastuacad(data);
        }

        [HttpPost]
        [Route("getgroupmappedheads")]
        public FeeStudentTransactionDTO getstuddetails([FromBody]FeeStudentTransactionDTO value)
        {
            return _org.getstuddet(value);
        }

        [Route("getgroupmappedheadsnew")]
        public FeeStudentTransactionDTO getstuddetailsnew([FromBody]FeeStudentTransactionDTO value)
        {
            return _org.getstuddetnew(value);
        }

        [Route("Deletedetails")]
        public FeeStudentTransactionDTO deletereceipt([FromBody]FeeStudentTransactionDTO data)
        {
            return _org.delrec(data);
        }

        [Route("feereceiptduplicate")]
        public FeeStudentTransactionDTO duplicaterece([FromBody]FeeStudentTransactionDTO data)
        {
            return _org.duplicaterecept(data);
        }

        [HttpPost]
        [Route("get_grp_reptno")]
        public FeeStudentTransactionDTO get_grp_reptno([FromBody] FeeStudentTransactionDTO data)
        {
            return _org.get_grp_reptno(data);
        }

        [Route("searchfilter")]
        public FeeStudentTransactionDTO searchfilter([FromBody] FeeStudentTransactionDTO data)
        {
            return _org.getsearchfilter(data);
        }


        [HttpPost]
        [Route("searching")]
        public FeeStudentTransactionDTO searching([FromBody] FeeStudentTransactionDTO data)
        {
            return _org.searching(data);
        }

        [Route("edittransaction")]
        public FeeStudentTransactionDTO tranedit([FromBody] FeeStudentTransactionDTO data)
        {
            return _org.edittra(data);
        }

        [Route("printreceiptnew")]
        public Task<FeeStudentTransactionDTO> printreceiptnew([FromBody] FeeStudentTransactionDTO data)
        {
            return _org.printreceiptnew(data);
        }
        [Route("Search_Chaln_No")]
        public FeeStudentTransactionDTO Search_Chaln_No([FromBody] FeeStudentTransactionDTO data)
        {
            return _org.Search_Chaln_No(data);
        }
        [Route("Save_Chaln_No")]
        public FeeStudentTransactionDTO Save_Chaln_No([FromBody] FeeStudentTransactionDTO data)
        {
            return _org.Save_Chaln_No(data);
        }


        [Route("SendEmail")]
        public FeeStudentTransactionDTO SendEmail([FromBody] FeeStudentTransactionDTO data)
        {
            return _org.SendEmail(data);
        }

        [Route("getduedates")]
        public FeeStudentTransactionDTO getduedates([FromBody] FeeStudentTransactionDTO data)
        {
            return _org.getduedates(data);
        }

        [Route("getheadwisedetails")]
        public FeeStudentTransactionDTO getheadwisedetails([FromBody] FeeStudentTransactionDTO data)
        {
            return _org.getheadwisedetails(data);
        }

        [Route("viewstatus")]
        public FeeStudentTransactionDTO viewstatus([FromBody] FeeStudentTransactionDTO data)
        {
            return _org.viewstatus(data);
        }
        [Route("viewpaydetails")]
        public FeeStudentTransactionDTO viewpaydetails([FromBody] FeeStudentTransactionDTO data)
        {
            return _org.viewpaydetails(data);
        }
        [Route("viewpayexcessdetails")]
        public FeeStudentTransactionDTO viewpayexcessdetails([FromBody] FeeStudentTransactionDTO data)
        {
            return _org.viewpayexcessdetails(data);
        }

        [Route("OBTransfer")]
        public FeeStudentTransactionDTO OBTransfer([FromBody] FeeStudentTransactionDTO data)
        {
            return _org.OBTransfer(data);
        }


        //Rebate apply
        [Route("rebateamountcalc")]
        public FeeStudentTransactionDTO rebateamountcalc([FromBody]FeeStudentTransactionDTO data)
        {
            return _org.rebateamountcalc(data);
        }

        [Route("Rebateapplyandsave")]
        public Task<FeeStudentTransactionDTO> Rebateapplyandsave([FromBody] FeeStudentTransactionDTO pgmodu)
        {
            return _org.Rebateapplyandsave(pgmodu);
        }

        //Readminssion insertion fees

        [Route("Readminssioninsertionfees")]
        public FeeStudentTransactionDTO Readminssioninsertionfees([FromBody]FeeStudentTransactionDTO data)
        {
            return _org.Readminssioninsertionfees(data);
        }

    }
}
