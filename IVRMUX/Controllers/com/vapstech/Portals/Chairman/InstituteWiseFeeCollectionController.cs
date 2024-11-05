﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
//using DomainModel.Model.com.vapstech.p;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using IVRMUX.Delegates.com.vapstech.Portals.Chairman;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class InstituteWiseFeeCollectionController : Controller
    {

        InstituteWiseFeeCollectionDelegate crStr = new InstituteWiseFeeCollectionDelegate();


        // GET api/values
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

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
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
        [HttpGet]
        [Route("Getdetails")]
        public InstituteWiseFeeCollectionDTO Getdetails(InstituteWiseFeeCollectionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return crStr.Getdetails(data);
        }


        [Route("getdata/{id}")]
        public InstituteWiseFeeCollectionDTO getdata(int id)
        {
            InstituteWiseFeeCollectionDTO data = new InstituteWiseFeeCollectionDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = id;
            return crStr.Getdetails(data);
        }

        [HttpPost]
        [Route("Getsectioncount")]
        public InstituteWiseFeeCollectionDTO Getsectioncount([FromBody] InstituteWiseFeeCollectionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            // data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return crStr.Getsectioncount(data);
        }

    }
}
