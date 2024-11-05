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
    public class FeeCardDetailsEntryFacade : Controller
    {
        public FeeCardDetailsEntryInterface _org;

        public FeeCardDetailsEntryFacade(FeeCardDetailsEntryInterface orga)
        {
            _org = orga;
        }

       
        [HttpPost]
        [Route("getdata")]
        public FeeCardDetailEntryDTO getdata([FromBody] FeeCardDetailEntryDTO data)
        {
            return _org.getdata(data);
        }

        [Route("searchfilter")]
        public FeeCardDetailEntryDTO searchfilter([FromBody] FeeCardDetailEntryDTO data)
        {
            return _org.getsearchfilter(data);
        }

        [Route("getstudlistgroup")]
        public FeeCardDetailEntryDTO getstudlistgroup([FromBody] FeeCardDetailEntryDTO data)
        {
            return _org.getstudlistgroup(data);
        }

        [Route("getgroupmappedheads")]
        public FeeCardDetailEntryDTO getgroupmappedheads([FromBody] FeeCardDetailEntryDTO data)
        {
            return _org.getgroupmappedheads(data);
        }

        [HttpPost]
        [Route("savedata")]
        public FeeCardDetailEntryDTO savedata([FromBody] FeeCardDetailEntryDTO pgmodu)
        {
            return _org.savedata(pgmodu);
        }
        [Route("editdetails/{id:int}")]
        public FeeCardDetailEntryDTO editdetails(int id)
        {
            return _org.editdetails(id);
        }

        [Route("Deletedetails/{id:int}")]
        public FeeCardDetailEntryDTO delete(int id)
        {
            return _org.Deletedetails(id);
        }       
    }
}
