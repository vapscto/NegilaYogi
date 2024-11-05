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
using CollegePortals.com.Chairman.Interfaces;
using PreadmissionDTOs.com.vaps.College.Portals;

namespace CollegePortals.com.Chairman.Controllers
{
    [Route("api/[controller]")]
    public class CLGCasteWiseStudentStrengthFacade : Controller
    {
        public CLGCasteWiseStudentStrengthInterface _ads;

        public CLGCasteWiseStudentStrengthFacade(CLGCasteWiseStudentStrengthInterface adstu)
        {
            _ads = adstu;
        }

        // POST api/values

        [HttpPost]
        [Route("Getdetails")]       
        public Task<CLGCHStudentStrengthDTO> Getdetails([FromBody]CLGCHStudentStrengthDTO data)
        {
            return _ads.Getdetails(data);
        }


    }
}
