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
    public class ArrearSalaryFacadeController : Controller
    {
        public ArrearSalaryReportInterface _ads;

        public ArrearSalaryFacadeController(ArrearSalaryReportInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public HR_Arrear_SalaryDTO getinitialdata([FromBody]HR_Arrear_SalaryDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        [Route("getEmployeedetailsBySelection")]
        public Task<HR_Arrear_SalaryDTO> getEmployeedetailsBySelection([FromBody]HR_Arrear_SalaryDTO dto)
        {
            return _ads.getEmployeedetailsBySelection(dto);
        }
        [Route("get_depts")]
        public HR_Arrear_SalaryDTO get_depts([FromBody]HR_Arrear_SalaryDTO dto)
        {
            return _ads.get_depts(dto);
        }
        [Route("get_desig")]
        public HR_Arrear_SalaryDTO get_desig([FromBody]HR_Arrear_SalaryDTO dto)
        {
            return _ads.get_desig(dto);
        }
    }
}

