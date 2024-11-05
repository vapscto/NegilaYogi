﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using WebApplication1.Interfaces;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class PremonthendreportFacadeController  : Controller
    {
        public PremonthendreportInterface _feetar;

        public PremonthendreportFacadeController(PremonthendreportInterface adstu)
        {
            _feetar = adstu;
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