﻿using System;
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
    public class SpecialFeeHeadClgFacade : Controller
    {
        SpecialFeeHeadClgInterface _feespecialhead;

        public SpecialFeeHeadClgFacade(SpecialFeeHeadClgInterface maspag)
        {
            _feespecialhead = maspag;
        }
        //// GET: api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

        public SpecialFeeHeadClgFacade _feegrouppage;
        [HttpPost]
        [Route("SaveYearlyGrpdata/")]
        public SpecialFeeHeadClgDTO SaveYearlyGrpdata([FromBody] SpecialFeeHeadClgDTO reg)
        {
            //  string str = "false";
            //    for (int i = 0; i < reg.TempararyArrayList.Length; i++)
            //    {
            //        int Id = Convert.ToInt32(reg.TempararyArrayList[i].FMH_ID);


            //        reg.FMH_ID = Id;


            //        _feespecialhead.SaveYearlyGroupDataY(Id, reg);
            ////    }

            return _feespecialhead.SaveYearlyGroupDataY(reg);
        }

        // GET api/values/5
        [Route("getdetailsY/{id:int}")]
        public SpecialFeeHeadClgDTO getorgdetY(int id)
        {
            return _feespecialhead.getdetailsY(id);
        }

        [HttpPost]
        [Route("deactivateY")]
        public SpecialFeeHeadClgDTO deactivateY([FromBody] SpecialFeeHeadClgDTO id)
        {
            // id = 12;
            return _feespecialhead.deactivateY(id);
        }

        [Route("getpagedetailsY/{id:int}")]
        //[Route("getenquirycontroller")]
        public SpecialFeeHeadClgDTO getpagedetailsY(int id)
        {
            // id = 12;
            return _feespecialhead.getpageeditY(id);
        }

        [HttpPost]
        [Route("deletedetailsY")]
        public SpecialFeeHeadClgDTO DeleterecY(SpecialFeeHeadClgDTO data)
        {
            return _feespecialhead.deleterecY(data);
        }
    }
}