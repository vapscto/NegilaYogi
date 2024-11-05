using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.Employee.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Employee;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Employee.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeStudentSearchFacade : Controller
    {

        public EmployeeStudentSearchInterface _org;
        public EmployeeStudentSearchFacade(EmployeeStudentSearchInterface org)
        {
            _org = org;
        }

        [Route("getalldetails")]
        public EmployeeDashboardDTO getalldetails([FromBody] EmployeeDashboardDTO data)
        {
            return _org.getdatastuacadgrp(data);
        }

        [Route("getstudentdetails")]
        public EmployeeDashboardDTO getstudentdetails([FromBody] EmployeeDashboardDTO data)
        {
            return _org.getstudentdetails(data);
        }


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}