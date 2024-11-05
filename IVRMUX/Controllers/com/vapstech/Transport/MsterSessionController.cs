﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.Transport;
using PreadmissionDTOs.com.vaps.Transport;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Transport
{
    [Route("api/[controller]")]
    public class MsterSessionController : Controller
    {
        MsterSessionDelegate _session = new MsterSessionDelegate();
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
        public MsterSessionDTO getdataP (int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _session.getdata(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [Route("savedata")]
        public MsterSessionDTO savedata([FromBody] MsterSessionDTO data)
        {
           data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _session.savedata(data);
        }

        [Route("edit")]
        public MsterSessionDTO edit([FromBody] MsterSessionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _session.edit(data);
        }
        [Route("activedeactive")]
        public MsterSessionDTO activedeactive([FromBody] MsterSessionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _session.activedeactive(data);
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