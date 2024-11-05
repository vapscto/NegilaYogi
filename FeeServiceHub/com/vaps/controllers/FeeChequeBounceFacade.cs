using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using PreadmissionDTOs.com.vaps.Fees;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeChequeBounceFacade : Controller
    {
        public FeeChequeBounceInterface _org;

        public FeeChequeBounceFacade(FeeChequeBounceInterface orga)
        {
            _org = orga;
        }

       
        [Route("getalldetails")]
        public FeeChequeBounceDTO Getdet([FromBody] FeeChequeBounceDTO data )
        {
            return _org.getdata(data);
        }

        [Route("getacademicyear")]
        public FeeChequeBounceDTO Getstudacademic([FromBody] FeeChequeBounceDTO data)
        {
            return _org.getdatastuacad(data);
        }

        [Route("getstudlistgroup")]
        public FeeChequeBounceDTO Getstudacademicgrp([FromBody] FeeChequeBounceDTO data)
        {
            return _org.getdatastuacadgrp(data);
        }

        [Route("getSchoolTypedetails")]
        public FeeChequeBounceDTO editdetails([FromBody] FeeChequeBounceDTO data)
        {
            return _org.geteditdet(data);
        }

        [HttpPost]
        public FeeChequeBounceDTO savedata([FromBody] FeeChequeBounceDTO pgmodu)
        {
            //int trustid = 0;
            //if (HttpContext.Session.GetString("pagemoduleid") != null)
            //{
            //    trustid = Convert.ToInt32(HttpContext.Session.GetString("pagemoduleid"));//Get
            //}

            //pgmodu.IVRMMP_Id = trustid;
            //HttpContext.Session.Remove("pagemoduleid");
            return _org.savedetails(pgmodu);
        }

        [HttpPost]
        [Route("getgroupmappedheads")]
        public FeeChequeBounceDTO getstuddetails([FromBody]FeeChequeBounceDTO value)
        {
            return _org.getstuddet(value);
        }


        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
     
        [Route("Deletedetails")]
        public FeeChequeBounceDTO deleterecord([FromBody] FeeChequeBounceDTO data)
        {
            return _org.deleterec(data);
        }
        [HttpPost]
        [Route("searching")]
        public FeeChequeBounceDTO searching([FromBody] FeeChequeBounceDTO data)
        {
            return _org.searching(data);
        }
        [Route("get_students")]
        public FeeChequeBounceDTO get_students([FromBody] FeeChequeBounceDTO data)
        {
            return _org.get_students(data);
        }
        [Route("get_section")]
        public FeeChequeBounceDTO get_section([FromBody] FeeChequeBounceDTO data)
        {
            return _org.get_section(data);
        }
        [Route("get_receipts")]
        public FeeChequeBounceDTO get_receipts([FromBody] FeeChequeBounceDTO data)
        {
            return _org.get_receipts(data);
        }
    }
}
