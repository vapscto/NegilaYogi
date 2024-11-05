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

namespace PortalHub.com.vaps.Student.Controllers
{
    [Route("api/[controller]")]
    public class UpdateRequestFacade : Controller
    {
        public UpdateRequestInterface _ads;

        public UpdateRequestFacade(UpdateRequestInterface adstu)
        {
            _ads = adstu;
        }

        [Route("getstudata")]
        public Task<UpdateRequestDTO> getstudata([FromBody]UpdateRequestDTO sddto)
        {
            return _ads.getstudata(sddto);
        }

        [HttpPost]
        [Route("getloaddata")]
        public Task<UpdateRequestDTO> getloaddata([FromBody]UpdateRequestDTO data)
        {
            return _ads.getloaddata(data);
        }

        [HttpPost]
        [Route("getreploaddata")]
        public Task<UpdateRequestDTO> getreploaddata([FromBody]UpdateRequestDTO data)
        {
            return _ads.getreploaddata(data);
        }

        [HttpPost]
        [Route("getreport")]
        public Task<UpdateRequestDTO> getreport([FromBody]UpdateRequestDTO data)
        {
            return _ads.getreport(data);
        }
        [HttpPost]
        [Route("saverequest")]
        public UpdateRequestDTO saverequest([FromBody]UpdateRequestDTO data)
        {
            return _ads.saverequest(data);
        }
        [Route("savedataadmin")]
        public UpdateRequestDTO savedataadmin([FromBody]UpdateRequestDTO data)
        {
            return _ads.savedataadmin(data);
        }
        [Route("guardianDetails")]
        public UpdateRequestDTO guardianDetails([FromBody]UpdateRequestDTO data)
        {
            return _ads.guardianDetails(data);
        }
        [Route("savereject")]
        public UpdateRequestDTO savereject([FromBody]UpdateRequestDTO data)
        {
            return _ads.savereject(data);
        }

        [Route("getdpstate/{id:int}")]
        public Task<UpdateRequestDTO> getdpstate(int id)
        {
            return _ads.getStateByCountry(id);
        }

        [Route("searchfilter")]
        public UpdateRequestDTO searchfilter([FromBody]UpdateRequestDTO sddto)
        {
            return _ads.searchfilter(sddto);
        }


    }
}
