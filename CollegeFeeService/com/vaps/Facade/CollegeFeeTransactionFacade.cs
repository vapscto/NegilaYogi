using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CollegeFeeService.com.vaps.interfaces;
using System.IO;
using System.Net;
using System.Text;
using MimeKit;
using MailKit.Net.Smtp;
using PreadmissionDTOs.com.vaps.College.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class CollegeFeeTransactionFacade : Controller
    {
        public CollegeFeeTransactionInterface _org;

        public CollegeFeeTransactionFacade(CollegeFeeTransactionInterface orga)
        {
            _org = orga;
        }

        [HttpPost]
        [Route("getalldetails")]
        public CollegeFeeTransactionDTO Getdet([FromBody] CollegeFeeTransactionDTO data)
        {
            return _org.getdata(data);
        }


        [Route("getstudlistgroup")]
        public CollegeFeeTransactionDTO Getstudacademicgrp([FromBody] CollegeFeeTransactionDTO data)
        {
            return _org.getdatastuacadgrp(data);
        }

        [Route("dynamicfinecalculation")]
        public CollegeFeeTransactionDTO dynamicfinecalculation([FromBody] CollegeFeeTransactionDTO data)
        {
            return _org.dynamicfinecalculation(data);
        }

        [Route("printreceipt")]
        public Task<CollegeFeeTransactionDTO> printreceipt([FromBody] CollegeFeeTransactionDTO data)
        {
            return _org.printreceipt(data);
        }


        [HttpPost]
        public Task<CollegeFeeTransactionDTO> savedata([FromBody] CollegeFeeTransactionDTO pgmodu)
        {
            return _org.savedetails(pgmodu);
        }

        [Route("getacademicyear")]
        public CollegeFeeTransactionDTO Getstudacademic([FromBody] CollegeFeeTransactionDTO data)
        {
            return _org.getdatastuacad(data);
        }

        [HttpPost]
        [Route("getgroupmappedheads")]
        public CollegeFeeTransactionDTO getstuddetails([FromBody]CollegeFeeTransactionDTO value)
        {
            return _org.getstuddet(value);
        }

        [Route("getgroupmappedheadsnew")]
        public CollegeFeeTransactionDTO getstuddetailsnew([FromBody]CollegeFeeTransactionDTO value)
        {
            return _org.getstuddetnew(value);
        }

        [Route("Deletedetails")]
        public CollegeFeeTransactionDTO deletereceipt([FromBody]CollegeFeeTransactionDTO data)
        {
            return _org.delrec(data);
        }

        [Route("feereceiptduplicate")]
        public CollegeFeeTransactionDTO duplicaterece([FromBody]CollegeFeeTransactionDTO data)
        {
            return _org.duplicaterecept(data);
        }

        [HttpPost]
        [Route("get_grp_reptno")]
        public CollegeFeeTransactionDTO get_grp_reptno([FromBody] CollegeFeeTransactionDTO data)
        {
            return _org.get_grp_reptno(data);
        }

        [Route("searchfilter")]
        public CollegeFeeTransactionDTO searchfilter([FromBody] CollegeFeeTransactionDTO data)
        {
            return _org.getsearchfilter(data);
        }


        [HttpPost]
        [Route("searching")]
        public CollegeFeeTransactionDTO searching([FromBody] CollegeFeeTransactionDTO data)
        {
            return _org.searching(data);
        }

        [Route("edittransaction")]
        public CollegeFeeTransactionDTO tranedit([FromBody] CollegeFeeTransactionDTO data)
        {
            return _org.edittra(data);
        }

        [Route("printreceiptnew")]
        public Task<CollegeFeeTransactionDTO> printreceiptnew([FromBody] CollegeFeeTransactionDTO data)
        {
            return _org.printreceiptnew(data);
        }
        [Route("Search_Chaln_No")]
        public CollegeFeeTransactionDTO Search_Chaln_No([FromBody] CollegeFeeTransactionDTO data)
        {
            return _org.Search_Chaln_No(data);
        }
        [Route("Save_Chaln_No")]
        public CollegeFeeTransactionDTO Save_Chaln_No([FromBody] CollegeFeeTransactionDTO data)
        {
            return _org.Save_Chaln_No(data);
        }
        [Route("viewstatus")]
        public CollegeFeeTransactionDTO viewstatus([FromBody] CollegeFeeTransactionDTO data)
        {
            return _org.viewstatus(data);
        }

    }
}
