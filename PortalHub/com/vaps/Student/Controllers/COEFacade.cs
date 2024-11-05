﻿using System;
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
using PortalHub.com.vaps.Student.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Student;

namespace PortalHub.com.vaps.Student.Controllers
{
    [Route("api/[controller]")]
    public class COEFacade : Controller
    {
        public COEInterface _ads;

        public COEFacade(COEInterface adstu)
        {
            _ads = adstu;
        }

        [HttpPost]
        [Route("getloaddata")]      
        public StudentDashboardDTO getloaddata([FromBody]StudentDashboardDTO data)
        {
            return _ads.getloaddata(data);
        }
        [HttpPost]
        [Route("getcoedata")]
        public StudentDashboardDTO getcoedata([FromBody]StudentDashboardDTO sddto)
        {
            return _ads.getcoedata(sddto);
        }

    }
}
