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

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class AdmissionImportFacade : Controller
    {
        public AdmissionImportInterface _ads;

        public AdmissionImportFacade(AdmissionImportInterface adstu)
        {
            _ads = adstu;
        }
        // GET: api/values
        [HttpPost]
        [Route("savedata")]
        public Task< ImportStudentWrapperDTO> getinitialdata([FromBody] ImportStudentWrapperDTO stud)
        {
            return  _ads.getdetails(stud);
        }
        [Route("checkvalidation")]
        public Task<ImportStudentWrapperDTO> checkvalidation([FromBody] ImportStudentWrapperDTO data)
        {
            return _ads.checkvalidation(data);
        }
    }
}
