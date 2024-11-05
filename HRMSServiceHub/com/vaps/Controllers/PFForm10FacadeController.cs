﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRMSServicesHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServicesHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class PFForm10FacadeController : Controller
    {
        // GET: api/values
        public PFForm10Interface _ads;

        public PFForm10FacadeController(PFForm10Interface adstu)
        {
          _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public PFReportsDTO getinitialdata([FromBody]PFReportsDTO dto)
        {
          return _ads.getBasicData(dto);
        }

        //FilterEmployeeData
        [Route("FilterEmployeeData")]
        public PFReportsDTO FilterEmployeeData([FromBody]PFReportsDTO dto)
        {
          return _ads.FilterEmployeeData(dto);
        }

        [Route("getEmployeedetailsBySelection")]
        public PFReportsDTO getEmployeedetailsBySelection([FromBody]PFReportsDTO dto)
        {
          return _ads.getEmployeedetailsBySelection(dto);
        }

        [Route("getEmployeedetailsBySelectionStjames")]
        public PFReportsDTO getEmployeedetailsBySelectionStjames([FromBody]PFReportsDTO dto)
        {
            return _ads.getEmployeedetailsBySelectionStjames(dto);
        }
    }
}