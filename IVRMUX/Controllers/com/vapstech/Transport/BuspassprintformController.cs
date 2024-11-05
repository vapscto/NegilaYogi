﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.Transport;
using PreadmissionDTOs.com.vaps.Transport;
using Microsoft.AspNetCore.Http;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Transport
{
    [Route("api/[controller]")]
    public class BuspassprintformController : Controller 
    {
        BuspassprintformDelegate _trapp = new BuspassprintformDelegate();
        private readonly DomainModelMsSqlServerContext _context;

        public BuspassprintformController(IOptions<FacadeUrl> settings, DomainModelMsSqlServerContext context)
        {
            _context = context;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("getdata/{id:int}")]
        public TransportApprovedDTO getdata(int id)
        {

            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _trapp.getdata(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [Route("searchdetails")]
        public TransportApprovedDTO searchdetails([FromBody] TransportApprovedDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string accountname = "";
            string accesskey = "";
            ReadTemplateFromAzure obj = new ReadTemplateFromAzure();

            var datatstu = _context.IVRM_Storage_path_Details.ToList();
            if (datatstu.Count() > 0)
            {
                accountname = datatstu.FirstOrDefault().IVRM_SD_Access_Name;
                accesskey = datatstu.FirstOrDefault().IVRM_SD_Access_Key;
            }


            string html = obj.getHtmlContentFromAzure(accountname, accesskey, "buspass/" + data.MI_Id, "Busform.html", 0);
            data.htmldata = html;
            return _trapp.searchdetails(data);
        }
        [Route("showmodaldetails")]
        public TransportApprovedDTO showmodaldetails([FromBody] TransportApprovedDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            string accountname = "";
            string accesskey = "";
            ReadTemplateFromAzure obj = new ReadTemplateFromAzure();

            var datatstu = _context.IVRM_Storage_path_Details.ToList();
            if (datatstu.Count() > 0)
            {
                accountname = datatstu.FirstOrDefault().IVRM_SD_Access_Name;
                accesskey = datatstu.FirstOrDefault().IVRM_SD_Access_Key;
            }


            string html = obj.getHtmlContentFromAzure(accountname, accesskey, "busform/" + data.MI_Id, "Busform.html", 0);
            data.htmldata = html;
            return _trapp.showmodaldetails(data);
        }

        [Route("savelist")]
        public TransportApprovedDTO savelist([FromBody] TransportApprovedDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _trapp.savelist(data);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}