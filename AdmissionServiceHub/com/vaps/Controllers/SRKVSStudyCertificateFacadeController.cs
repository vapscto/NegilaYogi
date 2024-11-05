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
    public class SRKVSStudyCertificateFacadeController : Controller
    {



            public SRKVSStudyCertificateInterface _ads;

            public SRKVSStudyCertificateFacadeController(SRKVSStudyCertificateInterface adstu)
            {
                _ads = adstu;
            }
            // GET: api/values
            [HttpGet]
            [Route("getdata/{id:int}")]
            public SRKVSStudycertificateDTO getinitialdata(int id)
            {
            SRKVSStudycertificateDTO stud = new SRKVSStudycertificateDTO();
                stud.MI_Id = id;
                return _ads.getdetails(stud);
            }


            [Route("getS")]
            public SRKVSStudycertificateDTO getstudentlist([FromBody]SRKVSStudycertificateDTO student)
            {
                return _ads.getstudlist(student);
            }

            [HttpPost]
            [Route("getStudData")]
            public Task<SRKVSStudycertificateDTO> Post([FromBody] SRKVSStudycertificateDTO studData)
            {
                return _ads.getStudDetails(studData);
            }
            [Route("onacademicyearchange")]
            public SRKVSStudycertificateDTO onacademicyearchange([FromBody] SRKVSStudycertificateDTO data)
            {
                return _ads.onacademicyearchange(data);
            }
            [Route("searchfilter")]
            public SRKVSStudycertificateDTO searchfilter([FromBody] SRKVSStudycertificateDTO data)
            {
                return _ads.searchfilter(data);
            }
            [Route("Studdetailsconduct")]
            public SRKVSStudycertificateDTO Studdetailsconduct([FromBody] SRKVSStudycertificateDTO data)
            {
                return _ads.Studdetailsconduct(data);
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

            // DELETE api/values/5
            [HttpDelete("{id}")]
            public void Delete(int id)
            {
            }
        }
    }
