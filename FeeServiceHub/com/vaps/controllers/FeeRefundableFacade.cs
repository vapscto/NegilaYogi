using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeRefundableFacade : Controller
    {
        public FeeRefundableInterface _org;

        public FeeRefundableFacade(FeeRefundableInterface orga)
        {
            _org = orga;
        }

     
        [Route("getalldetails")]
        public FeeMasterRefundDTO Getdet([FromBody] FeeMasterRefundDTO id)
        {
            return _org.getdata(id);
        }

        [Route("getacademicyear")]
        public FeeMasterRefundDTO Getstudacademic([FromBody] FeeMasterRefundDTO id)
        {
            return _org.getdatastuacad(id);
        }

        [Route("getstudlistgroup")]
        public FeeMasterRefundDTO Getstudacademicgrp([FromBody] FeeMasterRefundDTO id)
        {
            return _org.getdatastuacadgrp(id);
        }

        [Route("editdetails")]
        public FeeMasterRefundDTO editdetails([FromBody] FeeMasterRefundDTO id)
        {
            return _org.geteditdet(id);
        }

        [Route("getclasswisestudentlst")]
        public FeeMasterRefundDTO getclswisestulst([FromBody] FeeMasterRefundDTO id)
        {
            return _org.getdataclawisestude(id);
        }

        [Route("getStudentdetailsByYear")]
        public FeeMasterRefundDTO getStudentdetailsByYear(FeeMasterRefundDTO id)
        {
            return _org.GetStudentListByYear(id);
        }

        [Route("GetSection")]
        public FeeMasterRefundDTO GetSection([FromBody]FeeMasterRefundDTO sct)
        {
            return _org.GetSection(sct);
        }

        [Route("onselectacademicyear")]
        public FeeMasterRefundDTO onselectacademicyear([FromBody]FeeMasterRefundDTO sct)
        {
            return _org.onselectacademicyear(sct);
        }

        [Route("GetStudent")]
        public FeeMasterRefundDTO GetStudent([FromBody]FeeMasterRefundDTO sct)
        {
            return _org.GetStudent(sct);
        }



        [Route("GetStudentListByamst")]
        public FeeMasterRefundDTO GetStudentListByamst([FromBody]FeeMasterRefundDTO sct)
        {
            return _org.GetStudentListByamst(sct);
        }

        [HttpPost]
        public FeeMasterRefundDTO savedata([FromBody] FeeMasterRefundDTO pgmodu)
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
        public FeeMasterRefundDTO getstuddetails([FromBody]FeeMasterRefundDTO value)
        {
            return _org.getgroupheaddetails(value);
        }


        [HttpPost]
        [Route("modeofpayment")]
        public FeeMasterRefundDTO getmodeofpaydata([FromBody]FeeMasterRefundDTO value)
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
        public FeeMasterRefundDTO deleterecord([FromBody] FeeMasterRefundDTO id)
        {
            return _org.deleterec(id);
        }
        [HttpPost]
        [Route("searching")]
        public FeeMasterRefundDTO searching([FromBody] FeeMasterRefundDTO data)
        {
            return _org.searching(data);
        }
        [Route("get_recepts")]
        public FeeMasterRefundDTO get_recepts([FromBody] FeeMasterRefundDTO data)
        {
            return _org.get_recepts(data);
        }
    }
}
