using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using DomainModel.Model;
using DataAccessMsSqlServerProvider;
using MailKit.Net.Smtp;
using MimeKit;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using CollegePortals.com.Student.Interfaces;
using PreadmissionDTOs.com.vaps.College.Portals;

namespace CollegePortals.com.Student.Controllers
{
    [Route("api/[controller]")]
    public class Clg_HODEmpSalaryFacade : Controller
    {
        public Clg_HODEmpSalaryInterface _ads;

        public Clg_HODEmpSalaryFacade(Clg_HODEmpSalaryInterface adstu)
        {
            _ads = adstu;
        }

        // POST api/values

        [HttpPost]
        [Route("Getdetails")]       
        public Task<Clg_HODEmpSalaryDTO> Getdetails([FromBody]Clg_HODEmpSalaryDTO data)
        {
            return _ads.Getdetails(data);
        }

      

    }
}
