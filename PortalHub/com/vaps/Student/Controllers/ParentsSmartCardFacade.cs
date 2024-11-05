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
    public class ParentsSmartCardFacade : Controller
    {
        public ParentsSmartCardInterface _ads;

        public ParentsSmartCardFacade(ParentsSmartCardInterface adstu)
        {
            _ads = adstu;
        }

        [Route("getstudata")]
        public Task<ParentSmartCardDTO> getstudata([FromBody]ParentSmartCardDTO sddto)
        {
            return _ads.getstudata(sddto);
        }

        [HttpPost]
        [Route("getloaddata")]
        public Task<ParentSmartCardDTO> getloaddata([FromBody]ParentSmartCardDTO data)
        {
            return _ads.getloaddata(data);
        }
        [HttpPost]
        [Route("savedata")]
        public ParentSmartCardDTO savedata([FromBody]ParentSmartCardDTO data)
        {
            return _ads.savedata(data);
        }
        [Route("savedataadmin")]
        public ParentSmartCardDTO savedataadmin([FromBody]ParentSmartCardDTO data)
        {
            return _ads.savedataadmin(data);
        }
        [Route("guardianDetails")]
        public ParentSmartCardDTO guardianDetails([FromBody]ParentSmartCardDTO data)
        {
            return _ads.guardianDetails(data);
        }
        [Route("getreport")]
        public ParentSmartCardDTO getreport([FromBody]ParentSmartCardDTO data)
        {
            return _ads.getreport(data);
        }

        [Route("getdpstate/{id:int}")]
        public Task<ParentSmartCardDTO> getdpstate(int id)
        {
            return _ads.getStateByCountry(id);
        }

        [Route("searchfilter")]
        public ParentSmartCardDTO searchfilter([FromBody]ParentSmartCardDTO sddto)
        {
            return _ads.searchfilter(sddto);
        }


    }
}
