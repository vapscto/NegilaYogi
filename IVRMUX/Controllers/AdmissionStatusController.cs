using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class AdmissionStatusController : Controller
    {
        AdmissionStatusDelegate asd = new AdmissionStatusDelegate();

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public AdmissionStatusDTO Get(int id)
        {
            return asd.getacademicstatusdata(id);
        }

        [Route("getdetails/{id:int}")]
        public AdmissionStatusDTO editdetails(int id)
        {
            return asd.editdetail(id);
        }

        // POST api/values
        [HttpPost]
        public AdmissionStatusDTO savedata([FromBody] AdmissionStatusDTO data)
        {

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            return asd.savedataa(data);
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("deletepages/{id:int}")]
        public AdmissionStatusDTO deletedata(int id)
        {
            AdmissionStatusDTO data = new AdmissionStatusDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            data.PAMST_Id = id;

            return asd.deletedata(data);
        }
    }
}
