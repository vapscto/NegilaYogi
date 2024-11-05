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
using PortalHub.com.vaps.HOD.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Chirman;

namespace PortalHub.com.vaps.HOD.Controllers
{
    [Route("api/[controller]")]
    public class HODAttendanceDetailsFacade : Controller
    {
        public HODAttendanceDetailsInterface _ads;

        public HODAttendanceDetailsFacade(HODAttendanceDetailsInterface adstu)
        {
            _ads = adstu;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        
        [Route("Getdetails")]
        public ADMAttendenceDTO Getdetails([FromBody] ADMAttendenceDTO data)
        {


            return _ads.Getdetails(data);

        }

       
        [Route("getclass")]
        public ADMAttendenceDTO getclass([FromBody] ADMAttendenceDTO data)
        {


            return _ads.getclass(data);

        }
       
        [Route("Getsection")]
        public ADMAttendenceDTO Getsection([FromBody] ADMAttendenceDTO data)
        {


            return _ads.Getsection(data);

        }
        
        [Route("GetAttendence")]
        public ADMAttendenceDTO GetAttendence([FromBody] ADMAttendenceDTO data)
        {


            return _ads.GetAttendence(data);

        }

        
        [Route("GetIndividualAttendence")]
        public Task<ADMAttendenceDTO> GetIndividualAttendence([FromBody] ADMAttendenceDTO data)
        {


            return _ads.GetIndividualAttendenceAsync(data);

        }

    }
}
