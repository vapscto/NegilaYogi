﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Library;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class LibraryMonthEndReportFacade : Controller
    {
        public LibraryMonthEndReportInterface _objInter;
        public LibraryMonthEndReportFacade(LibraryMonthEndReportInterface para)
        {
            _objInter = para;
        }

        [Route("getdetails/{id:int}")]
        public LibraryMonthEndReportDTO getdetails(int id)
        {
            return _objInter.getdetails(id);
        }
        [Route("Savedata")]
        public LibraryMonthEndReportDTO Savedata([FromBody] LibraryMonthEndReportDTO data)
        {
            return _objInter.Savedata(data);
        }
       
    }
}
