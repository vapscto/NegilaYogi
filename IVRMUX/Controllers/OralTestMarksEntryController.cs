using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model;
using PreadmissionDTOs;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]

    public class OralTestMarksEntryController : Controller
    {
        // GET: /<controller>/
        OralTestMarksEntryDelegates OralTestMarksEntryDelegates = new OralTestMarksEntryDelegates();

        //// GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [Route("Getdetails/{id:int}")]
        public OralTestMarksBindDataDTO Getdetails(int ID)
        {
            OralTestOralByMarksDTO OralTestOralByMarksDTO = new OralTestOralByMarksDTO();

            OralTestOralByMarksDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));


            OralTestOralByMarksDTO.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
           

            return OralTestMarksEntryDelegates.GetOralTestMarksEntryData(OralTestOralByMarksDTO);
        }



        [HttpPost]
        public OralTestOralByMarksDTO OralTestMarksEntry([FromBody] OralTestOralByMarksDTO MMD)
        {

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.MI_Id = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            MMD.ASMAY_Id = ASMAY_Id;
            return OralTestMarksEntryDelegates.OralTestMarksEntryData(MMD);
        }

        [Route("GetdetailsOnSchedule/")]
        public OralTestOralByMarksDTO GetdetailsOnSchedule([FromBody] OralTestOralByMarksDTO MMD)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.MI_Id = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            MMD.ASMAY_Id = ASMAY_Id;

            return OralTestMarksEntryDelegates.GetdetailsOnSchedule(MMD);
        }

        [Route("GetOralTestMarks/")]
        public OralTestMarksBindDataDTO[] GetOralTestMarks([FromBody] OralTestMarksBindDataDTO MMD)
        {

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.MI_Id = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            MMD.ASMAY_Id = ASMAY_Id;

            return OralTestMarksEntryDelegates.GetOralTestMarks(MMD);
        }
    }
}
