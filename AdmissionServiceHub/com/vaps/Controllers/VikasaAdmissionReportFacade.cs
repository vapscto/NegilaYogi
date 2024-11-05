using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdmissionServiceHub.com.vaps.Interfaces;
using AdmissionServiceHub.com.vaps.Services;
using PreadmissionDTOs;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.admission;

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class VikasaAdmissionReportFacade :Controller
    {
        public VikasaAdmissionReportInterface _vikasaadmission;
        public VikasaAdmissionReportFacade(VikasaAdmissionReportInterface detail)
        {
            _vikasaadmission = detail;
        }
        // GET: api/values
        [HttpGet]
        [Route("getdata/{id:int}")]
        public VikasaAdmissionreportDTO getdetails(int id)
        {
            VikasaAdmissionreportDTO stud = new VikasaAdmissionreportDTO();
            stud.MI_Id = id;
            return _vikasaadmission.getdetails(stud); 
        }

        [Route("onacademicyearchange")]
        public VikasaAdmissionreportDTO onacademicyearchange([FromBody] VikasaAdmissionreportDTO data)
        {
            return _vikasaadmission.onacademicyearchange(data);
        }

        [HttpPost]
        [Route("getstudbyclass")]
        public VikasaAdmissionreportDTO getStudDatabyclass([FromBody] VikasaAdmissionreportDTO data)
        {
            return _vikasaadmission.getStudDatabyclass(data);
        }       

        [HttpPost]
        [Route("getStudData")]
        public Task<VikasaAdmissionreportDTO> Post([FromBody] VikasaAdmissionreportDTO studData)
        {
            return _vikasaadmission.getStudDetails(studData);
        }

        [Route("searchfilter")]
        public VikasaAdmissionreportDTO searchfilter([FromBody] VikasaAdmissionreportDTO studData)
        {
            return _vikasaadmission.searchfilter(studData);
        }
        [Route("ShowReport")]
        public VikasaAdmissionreportDTO ShowReport([FromBody] VikasaAdmissionreportDTO data)
        {
            return _vikasaadmission.ShowReport(data);
        }
        [Route("ShowReport1")]
        public VikasaAdmissionreportDTO ShowReport1([FromBody] VikasaAdmissionreportDTO data)
        {
            return _vikasaadmission.ShowReport1(data);
        }
        
        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
    }
}
