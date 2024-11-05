﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeServiceHub.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class ChangeOfBranchReportFacade : Controller
    {
        public ChangeOfBranchReportInterface _inter;
        public ChangeOfBranchReportFacade(ChangeOfBranchReportInterface p)
        {
            _inter = p;
        }

        [Route("loaddata")]
        public ChangeOfBranchReportDTO loaddata([FromBody] ChangeOfBranchReportDTO data)
        {
            return _inter.loaddata(data);
        }
        [Route("getcourse")]
        public ChangeOfBranchReportDTO getcourse([FromBody] ChangeOfBranchReportDTO data)
        {
            return _inter.getcourse(data);
        }
        [Route("getbranch")]
        public ChangeOfBranchReportDTO getbranch([FromBody] ChangeOfBranchReportDTO data)
        {
            return _inter.getbranch(data);
        }
        [Route("Report")]
        public Task<ChangeOfBranchReportDTO> Report([FromBody] ChangeOfBranchReportDTO data)
        {
            return _inter.Report(data);
        }
    }
}