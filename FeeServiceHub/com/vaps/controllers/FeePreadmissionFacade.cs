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
    public class FeePreadmissionFacade : Controller
    {
        public FeePreadmissionInterface _org;

        public FeePreadmissionFacade(FeePreadmissionInterface orga)
        {
            _org = orga;
        }

        [HttpPost]
        [Route("getalldetails")]
        public FeeStudentTransactionDTO Getdet([FromBody] FeeStudentTransactionDTO data)
        {
            return _org.getdata(data);
        }

        [Route("selectstudent")]
        public FeeStudentTransactionDTO selectstud([FromBody] FeeStudentTransactionDTO data)
        {
            return _org.selectstu(data);
        }

        [Route("getgroupmappedheadsnew_st")]
        public FeeStudentTransactionDTO selectgrptrm([FromBody] FeeStudentTransactionDTO data)
        {
            return _org.selectgrouppterm(data);
        }

        [Route("savedata_st")]
        public FeeStudentTransactionDTO savedata([FromBody] FeeStudentTransactionDTO data)
        {
            return _org.savedata(data);
        }

        [Route("printreceipt_s")]
        public FeeStudentTransactionDTO printrec([FromBody] FeeStudentTransactionDTO data)
        {
            return _org.printrec(data);
        }

        [Route("searchfilter")]
        public FeeStudentTransactionDTO search([FromBody] FeeStudentTransactionDTO data)
        {
            return _org.search(data);
        }

        [Route("searching_s")]
        public FeeStudentTransactionDTO searchfilter([FromBody] FeeStudentTransactionDTO data)
        {
            return _org.searchfilter(data);
        }

        [Route("get_grp_reptno")]
        public FeeStudentTransactionDTO recnogen([FromBody] FeeStudentTransactionDTO data)
        {
            return _org.recnogen(data);
        }

        [Route("Deletedetails_s")]
        public FeeStudentTransactionDTO deleterec([FromBody] FeeStudentTransactionDTO data)
        {
            return _org.delrec(data);
        }

        [Route("filterstudent")]
        public FeeStudentTransactionDTO filstude([FromBody] FeeStudentTransactionDTO data)
        {
            return _org.filstude(data);
        }

    }
}
