using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CollegeFeeService.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.College.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class CLGFeeRefundableFacade : Controller
    {
        public CLGFeeRefundableInterface _org;

        public CLGFeeRefundableFacade(CLGFeeRefundableInterface orga)
        {
            _org = orga;
        }

     
        [Route("getalldetails")]
        public CLGFeeRefundableDTO Getdet([FromBody] CLGFeeRefundableDTO id)
        {
            return _org.getdata(id);
        }

        [Route("getacademicyear")]
        public CLGFeeRefundableDTO Getstudacademic([FromBody] CLGFeeRefundableDTO id)
        {
            return _org.getdatastuacad(id);
        }

        [Route("getstudlistgroup")]
        public CLGFeeRefundableDTO Getstudacademicgrp([FromBody] CLGFeeRefundableDTO id)
        {
            return _org.getdatastuacadgrp(id);
        }

        [Route("editdetails")]
        public CLGFeeRefundableDTO editdetails([FromBody] CLGFeeRefundableDTO id)
        {
            return _org.geteditdet(id);
        }

        [Route("getclasswisestudentlst")]
        public CLGFeeRefundableDTO getclswisestulst([FromBody] CLGFeeRefundableDTO id)
        {
            return _org.getdataclawisestude(id);
        }

        [Route("getStudentdetailsByYear")]
        public CLGFeeRefundableDTO getStudentdetailsByYear([FromBody] CLGFeeRefundableDTO id)
        {
            return _org.GetStudentListByYear(id);
        }

        [Route("GetSection")]
        public CLGFeeRefundableDTO GetSection([FromBody]CLGFeeRefundableDTO sct)
        {
            return _org.GetSection(sct);
        }
        [Route("get_semisters")]
        public CLGFeeRefundableDTO get_semisters([FromBody]CLGFeeRefundableDTO sct)
        {
            return _org.get_semisters(sct);
        }
        [Route("GetStudent")]
        public CLGFeeRefundableDTO GetStudent([FromBody]CLGFeeRefundableDTO sct)
        {
            return _org.GetStudent(sct);
        }



        [Route("GetStudentListByamst")]
        public CLGFeeRefundableDTO GetStudentListByamst([FromBody]CLGFeeRefundableDTO sct)
        {
            return _org.GetStudentListByamst(sct);
        }

        [HttpPost]
        public CLGFeeRefundableDTO savedata([FromBody] CLGFeeRefundableDTO pgmodu)
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
        [Route("onselectgroup")]
        public CLGFeeRefundableDTO getstuddetails([FromBody]CLGFeeRefundableDTO value)
        {
            return _org.getgroupheaddetails(value);
        }


        [HttpPost]
        [Route("modeofpayment")]
        public CLGFeeRefundableDTO getmodeofpaydata([FromBody]CLGFeeRefundableDTO value)
        {
            return _org.getmodeofpaymentdata(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        
        [Route("Deletedetails")]
        public CLGFeeRefundableDTO deleterecord([FromBody] CLGFeeRefundableDTO id)
        {
            return _org.deleterec(id);
        }
        [HttpPost]
        [Route("searching")]
        public CLGFeeRefundableDTO searching([FromBody] CLGFeeRefundableDTO data)
        {
            return _org.searching(data);
        }
    }
}
