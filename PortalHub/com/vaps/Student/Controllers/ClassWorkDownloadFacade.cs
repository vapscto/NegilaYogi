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
using PreadmissionDTOs.com.vaps.Portals.Employee;

namespace PortalHub.com.vaps.Student.Controllers
{
    [Route("api/[controller]")]
    public class ClassWorkDownloadFacade : Controller
    {
        public ClassWorkDownloadInterface _cdi;

        public ClassWorkDownloadFacade(ClassWorkDownloadInterface cdi)
        {
            _cdi = cdi;
        }

        [HttpPost]
        [Route("getloaddata")]      
        public IVRM_ClassWorkDTO getloaddata([FromBody]IVRM_ClassWorkDTO data)
        {
            return _cdi.getloaddata(data);
        }
        [HttpPost]
        [Route("getwork")]
        public IVRM_ClassWorkDTO getwork([FromBody]IVRM_ClassWorkDTO data)
        {
            return _cdi.getwork(data);
        }

    }
}
