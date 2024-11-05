﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Exam;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Exam
{
    [Route("api/[controller]")]
    public class VikasaSchoolTermWiseSubjectCumulativeReportController : Controller
    {
        VikasaSchoolTermWiseSubjectCumulativeReportDelegates crStr = new VikasaSchoolTermWiseSubjectCumulativeReportDelegates();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("Getdetails")]
        public VikasaSubjectwiseCumulativeReportDTO Getdetails(VikasaSubjectwiseCumulativeReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.Getdetails(data);
        }


        [Route("showdetails")]
        public  VikasaSubjectwiseCumulativeReportDTO showdetails([FromBody] VikasaSubjectwiseCumulativeReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.showdetails(data);
        }

        [Route("get_class")]
        public VikasaSubjectwiseCumulativeReportDTO get_class([FromBody]VikasaSubjectwiseCumulativeReportDTO data)

        {

            //data.Amst_Id = Id;
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;           
            return crStr.get_class(data);
        }
        [Route("get_section")]
        public VikasaSubjectwiseCumulativeReportDTO get_section([FromBody]VikasaSubjectwiseCumulativeReportDTO data)

        {

            //data.Amst_Id = Id;        
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return crStr.get_section(data);
        }
        //[Route("get_Exam")]
        //public VikasaSubjectwiseCumulativeReportDTO get_Exam([FromBody]VikasaSubjectwiseCumulativeReportDTO data)

        //{

        //    //data.Amst_Id = Id;

        //    int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    data.MI_Id = mid;
        //    return crStr.get_Exam(data);
        //}

        [Route("get_subject")]
        public VikasaSubjectwiseCumulativeReportDTO get_subject([FromBody]VikasaSubjectwiseCumulativeReportDTO data)

        {

            //data.Amst_Id = Id;       
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return crStr.get_subject(data);
        }
    }
}
