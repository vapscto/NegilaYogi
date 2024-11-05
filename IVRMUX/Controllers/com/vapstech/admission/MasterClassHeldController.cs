using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.admission;
using PreadmissionDTOs.com.vaps.admission;

namespace corewebapi18072016.Controllers.com.vapstech.admission
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class MasterClassHeldController : Controller
    {
        MasterClassHeldDelegate mchd = new MasterClassHeldDelegate();
        // GET: api/MasterClassHeld
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

      
        [Route("getAllData/{id:int}")]
        public MasterClassHeldDTO Getdta(int id)
        {
            MasterClassHeldDTO mch = new MasterClassHeldDTO();
            mch.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            mch.AMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return mchd.getaldata(mch);
        }
        // GET: api/MasterClassHeld/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/MasterClassHeld
        [HttpPost]
        public MasterClassHeldDTO Post([FromBody]MasterClassHeldDTO mstDto)
        {
            mstDto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mchd.saveData(mstDto);
        }
        [Route("getClassHeld")]
        public MasterClassHeldDTO getNoOfClassHeld([FromBody]MasterClassHeldDTO mstDto)
        {
            mstDto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mchd.getNoOfClassHeld(mstDto);
        }
        // PUT: api/MasterClassHeld/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
