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

namespace CollegePortals.com.vaps.Student.Controllers
{
    [Route("api/[controller]")]
    public class ClgFeeDetailsFacade : Controller
    {
        public ClgFeeDetailsInterface _ads;

        public ClgFeeDetailsFacade(ClgFeeDetailsInterface adstu)
        {
            _ads = adstu;
        }


        [HttpPost]
        [Route("getloaddata")]      
        public ClgPortalFeeDTO getloaddata([FromBody]ClgPortalFeeDTO data)
        {
            return _ads.getloaddata(data);
        }

        [HttpPost]
        [Route("Getdetails")]
        public Task<ClgPortalFeeDTO> Getdetails([FromBody]ClgPortalFeeDTO data)
        {
            return _ads.Getdetails(data);
        }
    }
}
