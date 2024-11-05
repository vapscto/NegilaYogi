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

    public class WrittenTestMarksEntryController : Controller
    {

        // GET: /<controller>/
        WrittenTestMarksEntryDelegates WrittenTestMarksEntryDelegates = new WrittenTestMarksEntryDelegates();

        //// GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [Route("Getdetails/{id:int}")]
        public WrittenTestMarksBindDataDTO Getdetails(int ID)
        {
            WrittenTestMarksBindDataDTO MMD = new WrittenTestMarksBindDataDTO();

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.MI_Id = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            MMD.ASMAY_Id = ASMAY_Id;

            return WrittenTestMarksEntryDelegates.GetWrittenTestMarksEntryData(MMD);
        }

        [HttpPost]
        public WirttenTestSubjectWiseMarksEntryDTO WrittenTestMarksEntry([FromBody] WirttenTestSubjectWiseMarksEntryDTO MMD)
        {

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.MI_Id = mid;


            //int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //MMD. = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            MMD.ASMAY_Id = ASMAY_Id;

            return WrittenTestMarksEntryDelegates.WrittenTestMarksEntryData(MMD);
        }

        [Route("GetdetailsOnSchedule/")]
        public WirttenTestSubjectWiseMarksEntryDTO GetdetailsOnSchedule([FromBody] WirttenTestSubjectWiseMarksEntryDTO MMD)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.MI_Id = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            MMD.ASMAY_Id = ASMAY_Id;

            return WrittenTestMarksEntryDelegates.GetdetailsOnSchedule(MMD);
        }

        [Route("GetWrittenTestMarks/")]
        public WrittenTestMarksBindDataDTO GetWrittenTestMarks([FromBody] WrittenTestMarksBindDataDTO MMD)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.MI_Id = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            MMD.ASMAY_Id = ASMAY_Id;

            return WrittenTestMarksEntryDelegates.GetWrittenTestMarks(MMD);
        }

    }
}
