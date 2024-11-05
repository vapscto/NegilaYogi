﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Fees.FinancialAccounting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees.FinancialAccounting;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Fees.FinancialAccounting
{
    [Route("api/[controller]")]
    public class AutoLedgerCreationController : Controller
    {
        AutoLedgerCreationDelegate od = new AutoLedgerCreationDelegate();
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public AutoLedgerCreationDTO Get(int id)
        {
            AutoLedgerCreationDTO pgmodu = new AutoLedgerCreationDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.MI_Id = mid;
            pgmodu.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            pgmodu.ASMAY_Id = ASMAY_Id;
            return od.getdata(pgmodu);
        }
        [HttpPost]
        [Route("edit")]
        public AutoLedgerCreationDTO edit([FromBody] AutoLedgerCreationDTO id)
        {
            id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            id.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            id.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.edit(id);
        }
        [Route("savedata")]
        public AutoLedgerCreationDTO savedata([FromBody] AutoLedgerCreationDTO pgmodu)
        {
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.savedetails(pgmodu);
        }
        //savedatatwo
        [Route("savedatatwo")]
        public AutoLedgerCreationDTO savedatatwo([FromBody] AutoLedgerCreationDTO pgmodu)
        {
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.savedatatwo(pgmodu);
        }
        [Route("Deletedetails")]
        public AutoLedgerCreationDTO Delete([FromBody]AutoLedgerCreationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.deleterec(data);
        }

        [Route("sectionchange")]
        public AutoLedgerCreationDTO sectionchange([FromBody] AutoLedgerCreationDTO pgmodu)
        {
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            pgmodu.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.sectionchange(pgmodu);
        }
    }
}
