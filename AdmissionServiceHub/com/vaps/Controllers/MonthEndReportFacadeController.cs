﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.admission;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class monthendreportFacadeController : Controller
    {
        public monthendreportInterface _feetar;

        public monthendreportFacadeController(monthendreportInterface maspag)
        {
            _feetar = maspag;
        }

     
      
        [HttpPost]
        [Route("getalldetails123")]
        public MonthEndReportDTO Getdet([FromBody] MonthEndReportDTO data)
        {
            return _feetar.getdata123(data);
        }

        [Route("getreport")]
        public Task<MonthEndReportDTO> getreport([FromBody] MonthEndReportDTO data)
        {
            return _feetar.getreport(data);
        }
        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
