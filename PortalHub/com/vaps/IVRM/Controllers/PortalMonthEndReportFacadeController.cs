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
using PortalHub.com.vaps.Student.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Student;
using PreadmissionDTOs.com.vaps.Portals.IVRM;
using PortalHub.com.vaps.IVRM.Interfaces;

namespace PortalHub.com.vaps.IVRM.Controllers
{
    [Route("api/[controller]")]
    public class PortalMonthEndReportFacadeController : Controller
    {
        public PortalMonthEndReportInterface _ads;

        public PortalMonthEndReportFacadeController(PortalMonthEndReportInterface adstu)
        {
            _ads = adstu;
        }

        [HttpPost]
        [Route("getloaddata")]
        public PortalMonthEndReportDTO getloaddata([FromBody]PortalMonthEndReportDTO data)
        {
            return _ads.getloaddata(data);
        }
        [Route("getmonthreport")]
        public Task<PortalMonthEndReportDTO> getmonthreport([FromBody]PortalMonthEndReportDTO data)
        {
            return _ads.getmonthreport(data);
        }

      

    }
}
