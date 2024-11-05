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

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vapstech.Controllers
{
    [Route("api/[controller]")]
    public class StudyCertificateFacade : Controller
    {
        public StudyCertificateInterface _ads;

        public StudyCertificateFacade(StudyCertificateInterface adstu)
        {
            _ads = adstu;
        }
        // GET: api/values
        [HttpGet]
        [Route("getdata/{id:int}")]
        public StudycertificateDTO getinitialdata(int id)
        {
            StudycertificateDTO stud = new StudycertificateDTO();
            stud.MI_Id = id;
            return _ads.getdetails(stud);
        }


        [Route("getS")]
        public StudycertificateDTO getstudentlist([FromBody]StudycertificateDTO student)
        {
            return _ads.getstudlist(student);
        }

        [HttpPost]
        [Route("getStudData")]
        public Task<StudycertificateDTO> Post([FromBody] StudycertificateDTO studData)
        {
            return _ads.getStudDetails(studData);
        }
        [Route("onacademicyearchange")]
        public StudycertificateDTO onacademicyearchange([FromBody] StudycertificateDTO data)
        {
            return _ads.onacademicyearchange(data);
        }
        [Route("searchfilter")]
        public StudycertificateDTO searchfilter([FromBody] StudycertificateDTO data)
        {
            return _ads.searchfilter(data);
        }
        [Route("Studdetailsconduct")]
        public StudycertificateDTO Studdetailsconduct([FromBody] StudycertificateDTO data)
        {
            return _ads.Studdetailsconduct(data);
        }


        //SRKVS Endorsement
        [Route("searchfilterSRKVS")]
        public StudycertificateDTO searchfilterSRKVS([FromBody] StudycertificateDTO data)
        {
            return _ads.searchfilterSRKVS(data);
        }

        [Route("getStudDetailsSRKVS")]
        public Task<StudycertificateDTO> getStudDetailsSRKVS([FromBody] StudycertificateDTO studData)
        {
            return _ads.getStudDetailsSRKVS(studData);
        }
    }
}
